namespace Gateway.Models.Responses;

public class MessageResponse
{
    private int id{ get; set; }
    private int receiverId{ get; set; }
    private int senderId{ get; set; }
    private String content{ get; set; } = null!;
    private DateTime date{ get; set; }
}