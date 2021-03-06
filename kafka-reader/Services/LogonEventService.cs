using LogonEvents.Models;
using LogonEvents.Services.Interfaces;
using MongoDB.Driver;

namespace LogonEvents.Services;

public class LogonEventService : ILogonEventService
{
    private readonly IMongoCollection<LogonEvent> _collection;
    public LogonEventService()
    {
        MongoClient client = new MongoClient("mongodb://localhost:27017");
        _collection = client.GetDatabase("ztiService").GetCollection<LogonEvent>("logEvents");
    }

    public async Task AddLogonEvent(LogonEvent logonEvent, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(logonEvent, null, cancellationToken);
    }

    public async Task<LastLogonTimeResponse> GetLastLogonTime(int userId, CancellationToken cancellationToken)
    {
        var lastLogon = await _collection.Find(x => x.UserId == userId)
            .SortByDescending(x => x.Time)
            .FirstOrDefaultAsync();
        return new LastLogonTimeResponse
        {
            LastLogonTime = lastLogon != null ? lastLogon.Time : null
        };
    }
}