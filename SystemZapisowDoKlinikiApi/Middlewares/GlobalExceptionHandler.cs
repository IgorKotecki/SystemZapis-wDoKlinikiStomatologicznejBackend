using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SystemZapisowDoKlinikiApi.Exceptions;

namespace SystemZapisowDoKlinikiApi.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(IProblemDetailsService problemDetailsService, ILogger<GlobalExceptionHandler> logger)
    {
        _problemDetailsService = problemDetailsService;
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (status, errorCode) = exception switch
        {
            ArgumentException => (StatusCodes.Status400BadRequest, "INVALID_ARGUMENT"),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "NOT_FOUND"),
            BusinessException be => (StatusCodes.Status400BadRequest, be.ErrorCode),
            _ => (StatusCodes.Status500InternalServerError, "INTERNAL_SERVER_ERROR")
        };

        httpContext.Response.StatusCode = status;

        _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails()
            {
                Type = errorCode,
                Title = errorCode,
                Detail = exception.Message,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
            }
        });
    }
}