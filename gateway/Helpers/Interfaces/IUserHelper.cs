using Gateway.Models.Entities;

namespace Gateway.Helpers.Interfaces;

public interface IUserHelper
{
    User GetUserFromHttpContext(HttpContext context);
}