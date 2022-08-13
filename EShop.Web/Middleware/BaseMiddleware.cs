using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace EShop.Web.Middleware
{
    public abstract class BaseMiddleware
    {
        protected ILogger<BaseMiddleware> _logger;

        //https://trailheadtechnology.com/aspnetcore-multi-tenant-tips-and-tricks/
        protected readonly RequestDelegate _next;
        protected BaseMiddleware(RequestDelegate next, ILogger<BaseMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        protected async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            string msg = exception.GetBaseException().StackTrace;
            string userMsg = "Operation Failed";
            int code = Status500InternalServerError;
            
            if (exception is UnauthorizedAccessException)
            {
                userMsg = msg = "UnauthorizedAccess";
                code = Status401Unauthorized;
            }

            _logger.LogError($"Application Exception: {msg}");
        }

    }
}
