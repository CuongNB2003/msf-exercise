using Newtonsoft.Json;

namespace MsfServer.HttpApi.Host.Middlewares
{
    public class CustomAuthenticationMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == 401 || context.Response.StatusCode == 403)
            {
                context.Response.ContentType = "application/json";
                var response = new
                {
                    status = context.Response.StatusCode,
                    error = context.Response.StatusCode == 401 ? "Token không hợp lệ hoặc không được cung cấp." : "Bạn không có quyền truy cập tài nguyên này.",
                    instance = context.Request.Path,
                };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }
    }

}
