namespace TodoWeb.Appllication.Middleware;

public class RateLimitMiddleware : IMiddleware
{
    private readonly ILogger<RateLimitMiddleware> _logger;
    private static readonly Dictionary<string, (DateTime Timestamp, int RequestCount)> _requestCounts = new();

    public RateLimitMiddleware(ILogger<RateLimitMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var clientIp = context.Connection.RemoteIpAddress?.ToString();

        if (clientIp != null)
        {
            // Check if IP already exists in the dictionary
            if (_requestCounts.ContainsKey(clientIp))
            {
                var (timestamp, count) = _requestCounts[clientIp];

                // If it's within 1 minute, allow up to 10 requests
                if (DateTime.UtcNow - timestamp <= TimeSpan.FromMinutes(1))
                {
                    if (count >= 10)
                    {
                        _logger.LogWarning($"Rate limit exceeded for IP: {clientIp}");
                        context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                        await context.Response.WriteAsync("Too many requests. Please try again later.");
                        return;
                    }

                    // Increment request count for the IP
                    _requestCounts[clientIp] = (timestamp, count + 1);
                }
                else
                {
                    // Reset count after 1 minute has passed
                    _requestCounts[clientIp] = (DateTime.UtcNow, 1);
                }
            }
            else
            {
                // Initialize request count for the IP
                _requestCounts[clientIp] = (DateTime.UtcNow, 1);
            }
        }

        // Continue to the next middleware or endpoint
        await next(context);
    }
}
