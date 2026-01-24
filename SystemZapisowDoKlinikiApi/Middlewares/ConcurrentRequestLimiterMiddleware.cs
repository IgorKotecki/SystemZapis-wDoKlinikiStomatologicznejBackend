using System.Collections.Concurrent;
using SystemZapisowDoKlinikiApi.Attributes;

namespace SystemZapisowDoKlinikiApi.Middlewares;

public class ConcurrentRequestLimiterMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> _ipLocks = new();

    public ConcurrentRequestLimiterMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var hasRateLimitAttribute = endpoint?.Metadata.GetMetadata<ConcurrentRequestLimitAttribute>() != null;
        if (!hasRateLimitAttribute)
        {
            await _next(context);
            return;
        }

        var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var semaphore = _ipLocks.GetOrAdd(ipAddress, _ => new SemaphoreSlim(1, 1));

        if (!await semaphore.WaitAsync(0))
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await context.Response.WriteAsJsonAsync(new
            {
                error = "TOO_MANY_REQUESTS"
            });
            return;
        }

        try
        {
            await _next(context);
        }
        finally
        {
            semaphore.Release();
        }
    }
}