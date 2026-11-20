
using ECommerceManagementAPI.Helper;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceManagementAPI.Helper
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
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
                _logger.LogError(ex, "An unexpected error occurred.");

                context.Response.ContentType = "application/problem+json";

                var (statusCode, title, detail) = ex switch
                {
                    NotFoundException =>
                        (404, "Not Found", ex.Message),

                    ConflictException =>
                        (409, "Conflict", ex.Message),

                    _ =>
                        (500, "Internal Server Error",
                        "An unexpected error occurred.")
                };

                context.Response.StatusCode = statusCode;

                var problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Title = title,
                    Detail = detail
                };

                problemDetails.Extensions["traceId"] =
                    context.TraceIdentifier;

                await context.Response.WriteAsJsonAsync(
                    problemDetails);
            }
        }
    }
}
