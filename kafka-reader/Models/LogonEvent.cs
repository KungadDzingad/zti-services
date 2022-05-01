using MongoDB.Bson.Serialization.Attributes;

namespace LogonEvents.Models;

[BsonIgnoreExtraElements]
public class LogonEvent
{
    public int UserId { get; set; }
    public DateTime Time { get; set; }
}