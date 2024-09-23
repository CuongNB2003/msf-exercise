using Microsoft.EntityFrameworkCore;
using MsfServer.Domain.Security;
using MsfServer.EntityFrameworkCore.Database;
using MsfServer.HttpApi;
using MsfServer.HttpApi.Host.Extensions;
using MsfServer.HttpApi.Host.Middlewares;

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

// cấu hình JWT
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
if (jwtSettings == null)
{
    throw new ArgumentNullException(nameof(jwtSettings), "JwtSettings không được để trống.");
}
builder.Services.AddSingleton(jwtSettings);

// Dịch vụ của các service application 
builder.Services.AddCustomServices(connectionString);

// Dịch vụ controller 
builder.Services.AddControllers()
    .AddApplicationPart(typeof(RolesController).Assembly)
    .AddApplicationPart(typeof(UsersController).Assembly);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Thêm middleware tùy chỉnh vào pipeline để xử lý ngoại lệ
app.UseMiddleware<CustomExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
