using Gateway.Models.Entities;

namespace Gateway.Helpers.Interfaces;

public interface IJwtHelper
{
    int? ValidateJwtToken(string? token);
    string GenerateJwtToken(User user);
    RefreshToken GenerateRefreshToken(int userId);
}
