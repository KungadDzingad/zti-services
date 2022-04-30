using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Gateway.Helpers.Interfaces;
using Gateway.Models;
using Gateway.Models.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Gateway.Helpers;

public class JwtHelper : IJwtHelper
{
    private readonly DbContext _context;
    public JwtHelper(DbContext context)
    {
        _context = context;
    }

    public string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable(Consts.TokenSecretEnvVarName) ?? throw new ArgumentException());
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken(int userId)
    {
        var refreshToken = new RefreshToken
        {
            Token = GetUniqueToken(),
            ExpirationTime = DateTime.UtcNow.AddDays(7),
            UserId = userId
        };

        return refreshToken;
    }

    public long? ValidateJwtToken(string? token)
    {
        if (token == null)
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable(Consts.TokenSecretEnvVarName) ?? throw new ArgumentException());
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

            return userId;
        }
        catch
        {
            return null;
        }
    }
    private string GetUniqueToken()
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var tokenIsUnique = !_context.Users.Any(u => u.RefreshTokens.Any(t => t.Token == token));

        if (!tokenIsUnique)
            return GetUniqueToken();
        
        return token;
    }
}
