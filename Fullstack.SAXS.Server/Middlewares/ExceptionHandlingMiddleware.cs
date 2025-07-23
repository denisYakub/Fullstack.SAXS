using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Fullstack.SAXS.Server.Middlewares
{
    internal class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;
        private readonly IHostEnvironment _env = env;

        private static readonly Action<ILogger, string, Exception?> _logErrorMessage =
            LoggerMessage.Define<string>(
                LogLevel.Error,
                new EventId(1003, "ErrorMessage"),
                "{Message}");

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logErrorMessage(_logger, ex.Message, ex);

                var (statusCode, message) = MapExceptionToResponse(ex);

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                var errorResponse = new
                {
                    error = GetErrorTitle(statusCode),
                    message = _env.IsDevelopment() ? message : "Something went wrong"
                };

                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response
                    .WriteAsync(json)
                    .ConfigureAwait(false);
            }
        }

        private static (int StatusCode, string Message) MapExceptionToResponse(Exception ex)
        {
            return ex switch
            {
                NotSupportedException => (400, ex.Message),
                ArgumentNullException => (400, ex.Message),
                FormatException => (400, ex.Message),
                OverflowException => (400, ex.Message),
                KeyNotFoundException => (404, ex.Message),
                InvalidOperationException => (409, ex.Message),
                UnauthorizedAccessException => (401, ex.Message),

                ValidationException ve => (400, string.Join("; ", ve.ValidationResult)),

                _ => (500, ex.Message)
            };
        }

        private static string GetErrorTitle(int statusCode) => statusCode switch
        {
            400 => "Bad Request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Not Found",
            409 => "Conflict",
            500 => "Internal Server Error",
            _ => "Error"
        };
    }
}
