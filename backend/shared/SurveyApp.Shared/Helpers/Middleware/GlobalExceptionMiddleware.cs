using Microsoft.AspNetCore.Http;       
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace SurveyApp.Shared.Helpers;
public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var (status, message) = ex switch
        {
            KeyNotFoundException => (404, ex.Message),
            UnauthorizedAccessException => (401, ex.Message),
            ArgumentException => (400, ex.Message),
            _ => (500, "Internal server error")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = status;

        var body = JsonSerializer.Serialize(new
        {
            error = message,
            statusCode = status,
            timestamp = DateTime.UtcNow
        });

        return context.Response.WriteAsync(body);
    }
}