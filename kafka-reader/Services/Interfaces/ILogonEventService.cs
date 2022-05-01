using LogonEvents.Models;

namespace LogonEvents.Services.Interfaces;

public interface ILogonEventService
{
    public Task AddLogonEvent(LogonEvent logonEvent, CancellationToken cancellationToken);
    public Task<LastLogonTimeResponse> GetLastLogonTime(int userId, CancellationToken cancellationToken);
}