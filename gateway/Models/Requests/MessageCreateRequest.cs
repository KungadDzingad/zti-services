namespace Gateway.Models.Requests;

public class MessageCreateRequest
{
    private int receiverId{ get; set; }
    private String content{ get; set; } = null!;
}