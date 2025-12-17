using Serilog;
using Serilog.Context;

namespace Api.Extensions
{
    public static class LoggingExtensions
    {
        public static void LogRequest(string method, string path, string userId = null)
        {
            Log.Information("HTTP Request: {Method} {Path} from {UserId}", method, path, userId ?? "Anonymous");
        }

        public static void LogResponse(string method, string path, int statusCode, long elapsed)
        {
            Log.Information("HTTP Response: {Method} {Path} returned {StatusCode} in {ElapsedMs}ms", method, path, statusCode, elapsed);
        }

        public static void LogDatabaseOperation(string operation, string entity, int? recordCount = null)
        {
            if (recordCount.HasValue)
                Log.Information("Database {Operation} on {Entity}: {RecordCount} records affected", operation, entity, recordCount);
            else
                Log.Information("Database {Operation} on {Entity}", operation, entity);
        }

        public static void LogBusinessLogic(string action, Dictionary<string, object>? context = null)
        {
            Log.Information("Business Logic: {Action} {@Context}", action, context ?? new Dictionary<string, object>());
        }

        public static void LogApplicationError(string action, Exception ex, Dictionary<string, object>? context = null)
        {
            Log.Error(ex, "Error in {Action} {@Context}", action, context ?? new Dictionary<string, object>());
        }

        public static IDisposable PushProperty(string key, object value)
        {
            return LogContext.PushProperty(key, value);
        }
    }
}
