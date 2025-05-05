using Microsoft.AspNetCore.Mvc.Filters;

namespace TodoWeb.Appllication.ActionFilters;

public class TestFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        throw new NotImplementedException();
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        throw new NotImplementedException();
    }
}