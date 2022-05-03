namespace WebApi.Controllers;

using System.Net;
using System.Security.Authentication;
using Confluent.Kafka;
using Gateway.Authorization;
using Gateway.Exceptions;
using Gateway.Helpers;
using Gateway.Helpers.Interfaces;
using Gateway.Models.Requests;
using Gateway.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/v1/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserHelper _userHelper;
    private readonly ILogger<UserController> _logger;

    public UserController(
        IUserService userService,
        IUserHelper userHelper,
        ILogger<UserController> logger)
    {
        _userService = userService;
        _userHelper = userHelper;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<IActionResult> LogOn(SignInRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _userService.Authenticate(request, cancellationToken);
            SetTokenCookie(response.RefreshToken);
            return Ok(response);
        }
        catch (AuthenticationException)
        {
            return Forbid();
        }

    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
    {
        try
        {
            var refreshToken = Request.Cookies[Consts.RefreshTokenCookieName];
            var response = await _userService.RefreshToken(refreshToken, cancellationToken);
            SetTokenCookie(response.RefreshToken);
            return Ok(response);
        }
        catch (NotAuthorizedException)
        {
            return Unauthorized();
        }
    }

    [AllowAnonymous]
    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUpAccount(SignUpRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _userService.AddUser(request, cancellationToken);
            return Ok();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [AllowAnonymous]
    [HttpGet("test")]
    public async Task<IActionResult> test(CancellationToken cancellationToken)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
            ClientId = Dns.GetHostName()
        };
        
        using (var producer = new ProducerBuilder<Null, string>(config).Build())
        {
            await producer.ProduceAsync("test2", new Message<Null, string> { Value = "message"});
        }
        return Ok();
    }

    private void SetTokenCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append(Consts.RefreshTokenCookieName, token, cookieOptions);
    }
}
