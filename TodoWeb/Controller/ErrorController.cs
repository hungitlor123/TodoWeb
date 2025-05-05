using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TodoWeb.Controller;

[ApiController]
[Route("[controller]")]
public class ErrorController : ControllerBase
{
    
    private readonly ILogger<ErrorController> _logger;
    
    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        var exceptionHandler  = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        var exception = exceptionHandler?.Error.Message;

        return new JsonResult(new
        {
            StatusCode = 500,
            Error = exception
        });
    }

}