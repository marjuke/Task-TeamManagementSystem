using System.Diagnostics;
using Serilog.Context;

namespace Api.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var userId = context.User?.FindFirst("sub")?.Value ?? "Anonymous";

            using (LogContext.PushProperty("UserId", userId))
            using (LogContext.PushProperty("RequestId", context.TraceIdentifier))
            {
                _logger.LogInformation(
                    "HTTP Request started: {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path
                );

                try
                {
                    await _next(context);
                }
                finally
                {
                    stopwatch.Stop();

                    _logger.LogInformation(
                        "HTTP Request completed: {Method} {Path} - Status {StatusCode} - Duration {ElapsedMs}ms",
                        context.Request.Method,
                        context.Request.Path,
                        context.Response.StatusCode,
                        stopwatch.ElapsedMilliseconds
                    );
                }
            }
        }
    }
}
