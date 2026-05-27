namespace Watchlist_Tracker.Middleware;

using System.Net;
using System.Text.Json;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new { message = "An error occurred", details = exception.Message };

        return exception switch
        {
            UnauthorizedAccessException =>
                SetResponse(context, HttpStatusCode.Forbidden, "You are not authorized to perform this action"),
            ArgumentException =>
                SetResponse(context, HttpStatusCode.BadRequest, exception.Message),
            InvalidOperationException =>
                SetResponse(context, HttpStatusCode.BadRequest, exception.Message),
            _ =>
                SetResponse(context, HttpStatusCode.InternalServerError, "An unexpected error occurred")
        };
    }

    private static Task SetResponse(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.StatusCode = (int)statusCode;
        var response = new { message };
        return context.Response.WriteAsJsonAsync(response);
    }
}

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionMiddleware>();
    }
}
