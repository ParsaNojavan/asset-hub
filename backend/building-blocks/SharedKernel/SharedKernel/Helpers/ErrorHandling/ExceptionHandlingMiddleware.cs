using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedKernel.Helpers.ErrorHandling
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
                _logger.LogError(ex, "Unhandled exception");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode;
            string code;

            switch (exception)
            {
                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    code = "Unauthorized";
                    break;

                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    code = "NotFound";
                    break;

                case ArgumentException:
                    statusCode = HttpStatusCode.BadRequest;
                    code = "BadRequest";
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    code = "ServerError";
                    break;
            }

            var response = new ErrorResponse
            {
                Code = code,
                Message = exception.Message,
                TraceId = context.TraceIdentifier
            };

            var json = JsonSerializer.Serialize(response);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(json);
        }
    }
}
