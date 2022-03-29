using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using GasB360_server.Models;

namespace GasB360_server.Helpers.Customer;

[AttributeUsage(AttributeTargets.Method)]
public class AllowAnonymousAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly IList<string> _roles;

    public AuthorizeAttribute(params string[] roles)
    {
        _roles = roles ?? new string[] { };
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata
            .OfType<AllowAnonymousAttribute>()
            .Any();
        if (allowAnonymous)
            return;

        var user = (AuthResponse)context.HttpContext.Items["user"]!;
        if (user == null)
        {
            context.Result = new JsonResult(new { message = "unauthorized" })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
        else if (!_roles.Contains(user.Role) && _roles.Any())
        {
            context.Result = new JsonResult(new { message = "forbidden" })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }
    }
}
