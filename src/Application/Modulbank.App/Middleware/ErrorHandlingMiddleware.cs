using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Modulbank.Shared.Exceptions;
using Newtonsoft.Json;

namespace Modulbank.App.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<ErrorHandlingMiddleware> logger)
        {
            var result = string.Empty;

            switch (exception)
            {
                case ApplicationApiException appException:
                    context.Response.StatusCode = (int) appException.Code;
                    result = JsonConvert.SerializeObject(new
                    {
                        errors = appException.Errors
                    });
                    break;
                case Exception ex:
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    logger.LogError(ex, "Unhandled Exception");
                    result = JsonConvert.SerializeObject(new
                    {
                        errors = "Internal Server Error (500)"
                    });
                    break;
            }

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(result ?? "{}");
        }
    }
}