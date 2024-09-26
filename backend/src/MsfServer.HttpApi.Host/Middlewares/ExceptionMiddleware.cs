using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MsfServer.Domain.Entities;
using MsfServer.Domain.Shared.Exceptions;
using MsfServer.Domain.Shared.Responses;
using MsfServer.EntityFrameworkCore.Database;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MsfServer.HttpApi.Host.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next, IOptions<ApiBehaviorOptions> options, IServiceProvider serviceProvider)
    {
        private readonly RequestDelegate _next = next;
        private readonly ApiBehaviorOptions _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        public async Task Invoke(HttpContext context)
        {
            // sử lý việc ghi log vào cơ sở dữ liệu
            var startTime = DateTime.UtcNow;
            context.Response.OnStarting(() =>
            {
                var duration = (int)(DateTime.UtcNow - startTime).TotalMilliseconds;
                var statusCode = context.Response.StatusCode;
                return CreateLog(context, statusCode, duration);
            });
            try
            {
                //bắt lỗi khi người dùng không truyền token hoặc token sai quyền
                await _next(context);
                if(context.Response.StatusCode == 401)
                {
                    string result = CreateProblemDetails(httpContext: context, statusCode: StatusCodes.Status401Unauthorized, error: "Token không hợp lệ hoặc không được cung cấp.");
                    await context.Response.WriteAsync(result);
                }
                if(context.Response.StatusCode == 403)
                {
                    string result = CreateProblemDetails(httpContext: context, statusCode: StatusCodes.Status403Forbidden, error: "Bạn không có quyền truy cập tài nguyên này.");
                    await context.Response.WriteAsync(result);
                }
            }
            catch (CustomException ex)
            {
                string result = CreateProblemDetails(httpContext: context, statusCode: ex.ErrorCode, error: ex.ErrorMessage);
                await context.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                string result = CreateProblemDetails(httpContext: context, statusCode: StatusCodes.Status500InternalServerError, error: ex.Message);
                await context.Response.WriteAsync(result);
            }

        }

        public string CreateProblemDetails(HttpContext httpContext,int? statusCode = null, string? error = null)
        {
            statusCode ??= 500;

            var customErrorException = new ResponseError
            {
                Status = statusCode,
                Error = error,
            };

            ApplyProblemDetailsDefaults(httpContext, customErrorException, statusCode.Value);
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode.Value;

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy() // chuyển đổi các tên thuộc tính thành dạng camelCase
                }
            };

            var result = JsonConvert.SerializeObject(customErrorException, settings);

            return result;
        }

        private static void ApplyProblemDetailsDefaults(HttpContext httpContext, ResponseError customErrorDetails, int statusCode)
        {
            ArgumentNullException.ThrowIfNull(httpContext);
            customErrorDetails.Instance = httpContext.Request.GetEncodedPathAndQuery();
            customErrorDetails.Status ??= statusCode;
        }
        public async Task CreateLog(HttpContext context, int statusCode, int duration)
        {
            string path = $"{context.Request.Path}{context.Request.QueryString}";
            var method = context.Request.Method;
            var clientIpAddress = context.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? context.Connection.RemoteIpAddress?.ToString();
            var userName = context.User.FindFirst("name")?.Value ?? "";
            var log = RequestLog.AddLogEntry(method, statusCode, path, clientIpAddress, userName, duration);
            // Kiểm tra nếu URL chứa "/api/log" và /swagger thì không lưu vào database
            if (!log.Url!.Contains("/api/log", StringComparison.OrdinalIgnoreCase) && !log.Url!.Contains("/swagger", StringComparison.OrdinalIgnoreCase))
            {
                await LogToDatabase(log);
            }

        }

        private async Task LogToDatabase(RequestLog log)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MsfServerDbContext>();
            await dbContext.RequestLogs.AddAsync(log);
            await dbContext.SaveChangesAsync();
        }

    }
}