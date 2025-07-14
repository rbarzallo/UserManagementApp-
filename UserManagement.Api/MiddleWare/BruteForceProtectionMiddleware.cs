using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace UserManagement.Api.Middleware
{
    public class BruteForceProtectionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly LoginAttemptService _loginAttemptService;

        public BruteForceProtectionMiddleware(RequestDelegate next, LoginAttemptService loginAttemptService)
        {
            _next = next;
            _loginAttemptService = loginAttemptService;
        }

        public async Task Invoke(HttpContext context)
        {
            // Solo aplicamos protección a rutas de login
            if (context.Request.Path.StartsWithSegments("/api/auth/login"))
            {
                var email = context.Request.Form["Email"].ToString();
                var ip = context.Connection.RemoteIpAddress?.ToString();

                var key = email + "|" + ip;

                if (_loginAttemptService.IsBlocked(key))
                {
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    await context.Response.WriteAsync("Demasiados intentos fallidos. Inténtalo más tarde.");
                    return;
                }
            }

            await _next(context);
        }
    }
}
