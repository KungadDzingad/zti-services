using Newtonsoft.Json;

namespace Gateway.Models.Responses;

public class PostsResponse
{
    [JsonProperty("id")]
    public int Id{ get; set; }
    [JsonProperty("user_id")]
    public int UserId{ get; set; }
    [JsonProperty("title")]
    public String Title{ get; set; } = null!;

    [JsonProperty("content")]
    public String Content{ get; set; } = null!;
    [JsonProperty("date")]
    public DateTime Date{ get; set; }
}