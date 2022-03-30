using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace TesteApi.WebAPI.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var exceptionReturn = GetUsableException(exception);
            var result = exceptionReturn != null ? exceptionReturn.Message : string.Empty;

            result = JsonConvert.SerializeObject(new { message = result });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }

        private static Exception GetUsableException(Exception erro)
        {
            if (erro is TargetInvocationException)
            {
                if (erro.InnerException != null)
                {
                    return GetUsableException(erro.InnerException);
                }

                return null;
            }
            else
            {
                return erro;
            }
        }
    }
}
