using FirstAidAPI.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace FirstAidAPI.Exceptions
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, errorResponse) = exception switch
            {
                BusinessException businessEx => (
                    HttpStatusCode.BadRequest,
                    new ErrorResponse
                    {
                        Success = false,
                        Message = businessEx.Message,
                        ErrorCode = "BUSINESS_ERROR"
                    }
                ),

                NotFoundException notFoundEx => (
                    HttpStatusCode.NotFound,
                    new ErrorResponse
                    {
                        Success = false,
                        Message = notFoundEx.Message,
                        ErrorCode = "NOT_FOUND"
                    }
                ),

                ValidationException validationEx => (
                    HttpStatusCode.BadRequest,
                    new ErrorResponse
                    {
                        Success = false,
                        Message = "Dữ liệu không hợp lệ",
                        ErrorCode = "VALIDATION_ERROR",
                        Errors = validationEx.Errors
                    }
                ),

                UnauthorizedException unauthorizedEx => (
                    HttpStatusCode.Unauthorized,
                    new ErrorResponse
                    {
                        Success = false,
                        Message = unauthorizedEx.Message,
                        ErrorCode = "UNAUTHORIZED"
                    }
                ),

                _ => (
                    HttpStatusCode.InternalServerError,
                    new ErrorResponse
                    {
                        Success = false,
                        Message = "Đã xảy ra lỗi hệ thống. Vui lòng thử lại sau.",
                        ErrorCode = "INTERNAL_ERROR"
                    }
                )
            };

            // Log chi tiết lỗi
            if (statusCode == HttpStatusCode.InternalServerError)
            {
                _logger.LogError(exception,
                    "Internal server error: {Message}. Path: {Path}",
                    exception.Message,
                    context.Request.Path);
            }
            else
            {
                _logger.LogWarning(exception,
                    "Client error ({StatusCode}): {Message}. Path: {Path}",
                    statusCode,
                    exception.Message,
                    context.Request.Path);
            }

            context.Response.StatusCode = (int)statusCode;

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(errorResponse, jsonOptions));
        }
    }

    // Model cho error response
    public class ErrorResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
        public object? Errors { get; set; }
    }
}