using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MsfServer.Domain.Entities;
using MsfServer.EntityFrameworkCore.Database;

namespace MsfServer.HttpApi.Host.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly IServiceProvider _serviceProvider;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger, IServiceProvider serviceProvider)
        {
            _next = next;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var startTime = DateTime.UtcNow;
            var path = context.Request.Path;
            var method = context.Request.Method;
            var clientIpAddress = context.Connection.RemoteIpAddress?.ToString();
            var userName = context.User.FindFirst("name")?.Value ?? "";

            context.Response.OnStarting(() =>
            {
                var duration = (int)(DateTime.UtcNow - startTime).TotalMilliseconds;
                var statusCode = context.Response.StatusCode;

                return CreateLog(path, method, statusCode, clientIpAddress, userName, duration);
            });
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex, "Đang có lỗi xảy ra.");
                throw;
            }
        }

        private async Task CreateLog(string path, string method, int statusCode, string? clientIpAddress, string? userName, int duration)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MsfServerDbContext>();
            var log = new RequestLog
            {
                Url = path,
                Method = method,
                StatusCode = statusCode,
                ClientIpAddress = clientIpAddress,
                UserName = userName,
                Duration = duration
            };
            dbContext.RequestLogs.Add(log);
            await dbContext.SaveChangesAsync();
        }
    }
}
