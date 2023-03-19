using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Todo.API.Middleware;

/// <summary>
/// Middleware to set timeout for ASP.NET Core
/// </summary>
public class MaximumRequestTimeoutMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<MaximumRequestTimeoutMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MaximumRequestTimeoutMiddleware"/> class.
    /// </summary>
    /// <param name="next"></param>
    /// <param name="logger"></param>
    public MaximumRequestTimeoutMiddleware(
        RequestDelegate next,
        ILogger<MaximumRequestTimeoutMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes middleware to cancel task when timeout
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            _logger.LogInformation("Begin invoking {className}. Timestamp: {dateTime}", nameof(MaximumRequestTimeoutMiddleware), DateTime.Now.ToString(CultureInfo.InvariantCulture));
            using CancellationTokenSource? timeoutSource = CancellationTokenSource.CreateLinkedTokenSource(context.RequestAborted);
            timeoutSource.CancelAfter(30000);
            context.RequestAborted = timeoutSource.Token;

            await _next(context);
        }
        catch (OperationCanceledException operationCanceledException)
        {
            _logger.LogError(operationCanceledException, "{APIName} hit timeout.", $"{context.Request.Method} {context.Request.Path}");

            context.Response.StatusCode = StatusCodes.Status502BadGateway;
            context.Response.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Json).ToString();

            // Note: same message in inner and outer layer
            await context.Response.WriteAsync(
                JsonSerializer.Serialize("Timeout"));
        }
    }
}
