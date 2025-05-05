using Microsoft.AspNetCore.Mvc.Filters;
using TodoWeb.Appllication.Services.CacheService;

namespace TodoWeb.Appllication.ActionFilters
{
    public class AuditFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;

            var method = request.Method;

            var path = request.Path;

            var args = context.ActionArguments;

            
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var result = context.Result;

            var statusCode = context.HttpContext.Response.StatusCode;
        }
    }
}
