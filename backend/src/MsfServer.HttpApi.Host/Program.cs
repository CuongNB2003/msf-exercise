using Microsoft.EntityFrameworkCore;
using MsfServer.EntityFrameworkCore.Database;
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

// Gọi các phương thức mở rộng để cấu hình Authentication và Authorization
builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.AddCustomAuthorization();

// Dịch vụ của các service application 
builder.Services.AddCustomServices(connectionString);

// Dịch vụ controller 
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Gọi cấu hình Swagger
builder.Services.AddSwaggerConfiguration();

// cấu hình cros
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

var app = builder.Build();

// Thêm middleware tùy chỉnh vào pipeline để xử lý ngoại lệ
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//thêm cấu hình cros ở đây
app.UseCors("AllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
