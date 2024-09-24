using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using MsfServer.Domain.Entities;
using MsfServer.EntityFrameworkCore.Database;

namespace MsfServer.HttpApi.Host.Middlewares
{
    public class UserActivityLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UserActivityLoggingMiddleware> _logger;
        private readonly IServiceProvider _serviceProvider;

        public UserActivityLoggingMiddleware(RequestDelegate next, ILogger<UserActivityLoggingMiddleware> logger, IServiceProvider serviceProvider)
        {
            _next = next;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path;
            var method = context.Request.Method;

var token = context.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogError("Token is null or empty");
                    context.Response.StatusCode = 400; // Bad Request
                    await context.Response.WriteAsync("Token is missing or invalid");
                    return;
                }

                var userId = GetUserIdFromToken(token);
                await LogUserActivity(context, userId);

            await _next(context);
        }

        private async Task LogUserActivity(HttpContext context, int userId)
        {
            var path = context.Request.Path;
            var method = context.Request.Method;
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MsfServerDbContext>();
            var log = new UserActivityLog
            {
                UserId = userId,
                Path = path,
                Method = method
            };
            dbContext.UserActivityLogs.Add(log);
            await dbContext.SaveChangesAsync();
        }

        public int GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.First(claim => claim.Type == "sub").Value;
            return int.Parse(userIdClaim);
        }
    }
}
