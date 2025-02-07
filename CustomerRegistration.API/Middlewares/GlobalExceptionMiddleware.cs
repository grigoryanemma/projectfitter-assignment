using System.Net;
using System.Text.Json;

using CustomerRegistration.Application.Common.Constants;
using CustomerRegistration.Application.Common.Enums;

namespace CustomerRegistration.Infrastructure.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode;
            string errorCode;
            string errorMessage = string.IsNullOrEmpty(exception.Message)
                ? "An unexpected error occurred."
                : exception.Message;

            (statusCode, errorCode) = exception switch
            {
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, ((int)ErrorCode.Unauthorized).ToString()),
                InvalidOperationException => (HttpStatusCode.BadRequest, ((int)ErrorCode.InvalidOperation).ToString()),
                KeyNotFoundException => (HttpStatusCode.NotFound, ((int)ErrorCode.NotFound).ToString()),
                _ => (HttpStatusCode.InternalServerError, ((int)ErrorCode.InternalError).ToString())
            };

            context.Response.StatusCode = (int)statusCode;
            context.Response.Headers[APP_HTTPHEADERS.CustomErrorCode] = errorCode;
            context.Response.Headers[APP_HTTPHEADERS.CustomErrorMessage] = errorMessage;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                ErrorCode = errorCode,
                Message = errorMessage
            };

            string jsonResponse = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
