using Microsoft.AspNetCore.Mvc.Filters;

namespace TodoWeb.Appllication.ActionFilters;

public class TestFilter : IActionFilter, IResultFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        Console.WriteLine("OnActionExecuted");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        Console.WriteLine("OnActionExecuting");
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        Console.WriteLine("OnActionExecuted");
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
        Console.WriteLine("OnActionExecuting");
    }
}