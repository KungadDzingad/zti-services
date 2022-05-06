namespace Gateway.Models.Requests;

public class PostCreateRequest
{
    public String Title{ get; set; } = null!;
    public String Content{ get; set; } = null!;
}