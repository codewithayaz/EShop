using System.Security.Claims;
using EShop.Core.Interfaces;

namespace EShop.Web.Middleware
{
    public class UserSessionMiddleware : BaseMiddleware
    {
        public UserSessionMiddleware(RequestDelegate next, 
            ILogger<UserSessionMiddleware> logger) : base(next, logger)
        {}

        public async Task InvokeAsync(HttpContext httpContext, IUserSession userSession)
        {
            try
            {
                var request = httpContext.Request;
                //First setup the userSession, then call next midleware
                if (httpContext.User.Identity.IsAuthenticated)
                {
                    userSession.UserId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    userSession.UserName = httpContext.User.Identity.Name;
                    
                    userSession.Roles = httpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
                    userSession.ExposedClaims = httpContext.User.Claims.Select(c => new KeyValuePair<string, string>(c.Type, c.Value)).ToList();
                }


                // Call the next delegate/middleware in the pipeline
                await _next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                // We can't do anything if the response has already started, just abort.
                if (httpContext.Response.HasStarted)
                {
                    _logger.LogWarning("A Middleware exception occurred, but response has already started!");
                    throw;
                }

                await HandleExceptionAsync(httpContext, ex);
                throw;
            }
        }
    }
}