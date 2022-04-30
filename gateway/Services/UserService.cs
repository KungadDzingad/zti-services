using Gateway.Exceptions;
using Gateway.Helpers.Interfaces;
using Gateway.Models;
using Gateway.Models.Entities;
using Gateway.Models.Requests;
using Gateway.Models.Responses;
using Gateway.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IJwtHelper _jwtHelper;
    private readonly Models.DbContext _context;

    public UserService(Models.DbContext context, IJwtHelper jwtHelper, ILogger<UserService> logger)
    {
        _context = context;
        _jwtHelper = jwtHelper;
        _logger = logger;
    }

    public async Task AddUser(SignUpRequest request, CancellationToken cancellationToken)
    {
        if (await _context.Users.AnyAsync(x => x.Username == request.UserName))
            throw new Exception();

        var user = new User
        {
            Username = request.UserName,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetUserById(int userId)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Id == userId);
    }

    public async Task<SignInResponse> RefreshToken(string? token, CancellationToken cancellationToken)
    {
        var user = GetUserByRefreshToken(token);
        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        if (refreshToken.ExpirationTime < DateTime.UtcNow)
            throw new NotAuthorizedException();

        var newRefreshToken = RotateRefreshToken(refreshToken, user);
        user.RefreshTokens.Add(newRefreshToken);

        await RemoveOldRefreshTokens(user);

        _context.Update(user);
        _context.SaveChanges();

        var jwtToken = _jwtHelper.GenerateJwtToken(user);

        var response = new SignInResponse
        {
            Token = jwtToken,
            RefreshToken = refreshToken.Token
        };

        return response;
    }

    private RefreshToken RotateRefreshToken(RefreshToken refreshToken, User user)
    {
        var newRefreshToken = _jwtHelper.GenerateRefreshToken(user.Id);
        return newRefreshToken;
    }

    private User GetUserByRefreshToken(string? token)
    {
        var user = _context.Users
            .Include(x => x.RefreshTokens)
            .SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

        if (user == null)
            throw new NotAuthorizedException();

        return user;
    }

    public async Task<SignInResponse> Authenticate(SignInRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(x => x.RefreshTokens)
            .FirstOrDefaultAsync(x => x.Username == request.UserName);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            throw new Exception();

        var jwtToken = _jwtHelper.GenerateJwtToken(user);
        var refreshToken = _jwtHelper.GenerateRefreshToken(user.Id);
        user.RefreshTokens.Add(refreshToken);

        await RemoveOldRefreshTokens(user);

        _context.Update(user);
        await _context.SaveChangesAsync();

        var response = new SignInResponse
        {
            Token = jwtToken,
            RefreshToken = refreshToken.Token
        };

        return response;
    }

    private async Task RemoveOldRefreshTokens(User user)
    {
        var refreshTokensToRemove = user.RefreshTokens.Where(x => x.ExpirationTime < DateTime.UtcNow).ToList();

        _context.RefreshTokens.RemoveRange(refreshTokensToRemove);
        await _context.SaveChangesAsync();
    }
}
