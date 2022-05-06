using Newtonsoft.Json;

namespace Gateway.Models.Responses;

public class MessageResponse
{
    [JsonProperty("id")]
    public int Id{ get; set; }
    [JsonProperty("receiverId")]
    public int ReceiverId{ get; set; }
    [JsonProperty("senderId")]
    public int SenderId{ get; set; }
    [JsonProperty("content")]
    public String Content{ get; set; } = null!;
    [JsonProperty("date")]
    public DateTime Date{ get; set; }
}