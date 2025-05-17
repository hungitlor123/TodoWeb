using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TodoWeb.Appllication.ActionFilters;

public class AuthorizationFilter : IAuthorizationFilter
{
    private readonly string[]_role;
    public AuthorizationFilter(string role)
    {
        _role = role.Split(',');
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userId = context.HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            context.Result = new StatusCodeResult(401);
            return;
        }
        
        var role = context.HttpContext.Session.GetString("Role");
        if (_role.Contains(role))
        {
            context.Result = new StatusCodeResult(403);
        }
    }   
    
}