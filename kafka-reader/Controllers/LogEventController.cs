namespace LogonEvents.Controllers;

using LogonEvents.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/users")]
public class LogonEventController : ControllerBase
{
    private readonly ILogonEventService _logonEventService;

    public LogonEventController(ILogonEventService logonEventService)
    {
        _logonEventService = logonEventService;
    }

    [HttpGet("last-logon")]
    public async Task<IActionResult> GetLastLogonTime(CancellationToken cancellationToken)
    {
        int userId;

        bool parseResult = int.TryParse(Request.Headers["user-id"], out userId);
        if (parseResult)
        {
            return Ok(await _logonEventService.GetLastLogonTime(userId, cancellationToken));
        }
        else
        {
            return Unauthorized();
        }
    }
}