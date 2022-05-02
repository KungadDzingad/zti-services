namespace Gateway.Models.Requests;

public class SignUpRequest
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}
