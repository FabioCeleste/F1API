using F1RestAPI.Services.IpConnectionCounts;

namespace F1RestAPI.Middlewares;

public class IPLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IIpConnectionCount _ipConnectionCount;

    public IPLoggingMiddleware(RequestDelegate next, IIpConnectionCount ipConnectionCount)
    {
        _next = next;
        _ipConnectionCount = ipConnectionCount;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var ipAddress = context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                  ?? context.Connection.RemoteIpAddress?.ToString();
            if (!string.IsNullOrEmpty(ipAddress))
            {
                await _ipConnectionCount.AddNewCountToConnections(ipAddress);
            }

            await _next(context);
        }
        catch (System.Exception)
        {
            await _next(context);
        }

    }
}