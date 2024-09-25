using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MsfServer.Domain.Security;
using MsfServer.EntityFrameworkCore.Database;
using MsfServer.HttpApi;
using MsfServer.HttpApi.Host.Extensions;
using MsfServer.HttpApi.Host.Middlewares;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Đọc chuỗi kết nối từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' is not found.");
}

// Cấu hình db context
builder.Services.AddDbContext<MsfServerDbContext>(options =>
    options.UseSqlServer(connectionString));

// Gọi các phương thức mở rộng để cấu hình Authentication và Authorization
builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.AddCustomAuthorization();

// Dịch vụ của các service application 
builder.Services.AddCustomServices(connectionString);

// Dịch vụ controller 
builder.Services.AddControllers()
    .AddApplicationPart(typeof(RolesController).Assembly)
    .AddApplicationPart(typeof(UsersController).Assembly)
    .AddApplicationPart(typeof(UserLogController).Assembly)
    .AddApplicationPart(typeof(AuthController).Assembly);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Gọi cấu hình Swagger
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

// Thêm middleware tùy chỉnh vào pipeline để xử lý ngoại lệ
app.UseMiddleware<CustomExceptionMiddleware>();
app.UseMiddleware<CustomAuthenticationMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // cấu hình lại tên api và điều hướng api
    //app.UseSwaggerUI(c =>
    //{
    //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    //    c.RoutePrefix = string.Empty;
    //});
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Bỏ qua xác thực cho Swagger
app.UseWhen(context => !context.Request.Path.StartsWithSegments("/swagger") &&
!context.Request.Path.StartsWithSegments("/v3/api-docs"), appBuilder =>
{
    appBuilder.UseMiddleware<LoggingMiddleware>();
});

app.MapControllers();

app.Run();
