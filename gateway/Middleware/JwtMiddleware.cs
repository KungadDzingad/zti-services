using Gateway.Helpers;
using Gateway.Helpers.Interfaces;
using Gateway.Services.Interfaces;

namespace Gateway.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserService userService, IJwtHelper jwtHelper)
    {
        var token = context.Request.Headers[Consts.AuthorizationHeaderName].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtHelper.ValidateJwtToken(token);
        if (userId != null)
        {
            context.Items[Consts.ContextItemUserInfoName] = await userService.GetUserById(userId.Value);
        }

        await _next(context);
    }
}
