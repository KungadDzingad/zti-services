namespace Gateway.Models.Responses;

public class SignInResponse
{
    public string Token { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}
