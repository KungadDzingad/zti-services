namespace Gateway.Models.Requests;

public class MessageCreateRequest
{
    public int ReceiverId{ get; set; }
    public String Content{ get; set; } = null!;
}