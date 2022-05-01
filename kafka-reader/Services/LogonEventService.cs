using LogonEvents.Models;
using LogonEvents.Services.Interfaces;

namespace LogonEvents.Services;

public class LogonEventService : ILogonEventService
{
    public Task AddLogonEvent(LogonEvent logonEvent, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<LastLogonTimeResponse> GetLastLogonTime(int userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}