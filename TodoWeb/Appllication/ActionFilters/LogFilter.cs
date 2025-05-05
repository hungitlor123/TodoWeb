using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TodoWeb.Appllication.ActionFilters
{
    public class LogFilter : IExceptionFilter
    {
        private readonly ILogger<LogFilter> _logger;
        private readonly LogLevel _logLevel;
        public LogFilter(ILogger<LogFilter> logger, LogLevel logLevel, int aaa)
        {
            _logger = logger;
            _logLevel = logLevel;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            

            var message = $"Exception: {exception.Message}";

            _logger.Log(_logLevel, message);

            context.Result = new ObjectResult(new
            {
                message = @"An error occurred while processing your request.
                            Please contact with admin for more infomation",
                error = exception.Message
            })
            {
                StatusCode = 500
            };


        }
    }
}
