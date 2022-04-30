namespace Gateway.Models.Requests;

public class SignInRequest
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}
