using Newtonsoft.Json;

namespace Gateway.Models.Responses;
public class LastLogonResponse
{
    [JsonProperty("lastLogonTime")]
    public DateTime? LastLogonTime { get; set; }
}