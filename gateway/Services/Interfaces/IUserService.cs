using Gateway.Models.Entities;
using Gateway.Models.Requests;
using Gateway.Models.Responses;

namespace Gateway.Services.Interfaces;

public interface IUserService
{
    Task AddUser(SignUpRequest request, CancellationToken cancellationToken);
    Task<SignInResponse> Authenticate(SignInRequest request, CancellationToken cancellationToken);
    Task<SignInResponse> RefreshToken(string? token, CancellationToken cancellationToken);
    Task<User?> GetUserById(int userId);
}