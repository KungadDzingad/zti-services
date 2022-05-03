using System.Security.Authentication;
using Gateway.Helpers.Interfaces;
using Gateway.Models.Entities;

namespace Gateway.Helpers;

public class UserHelper : IUserHelper
{
    public User GetUserFromHttpContext(HttpContext context)
    {
        return (User)(context.Items[Consts.ContextItemUserInfoName] ?? throw new AuthenticationException());
    }
}
