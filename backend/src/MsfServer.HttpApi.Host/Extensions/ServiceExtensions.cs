using MsfServer.Application.Contracts.User;
using MsfServer.Domain.Shared.Responses;
using MsfServer.Application.Repositorys;
using MsfServer.Application.Contracts.Services;
using MsfServer.Application.Services;
using MsfServer.Application.Contracts.Authentication.AuthDtos;
using MsfServer.Application.Contracts.Authentication;
using MsfServer.Application.Contracts.Role;
using MsfServer.Application.Contracts.Token;
using MsfServer.Application.Contracts.Log;
using MsfServer.Application.Dapper;
using MsfServer.Application.Contracts.Role.RoleDtos;

namespace MsfServer.HttpApi.Host.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddCustomServices(this IServiceCollection services, string connectionString)
        {
            // những service sử dụng AddHttpClient sử dụng api bên thứ ba
            services.AddHttpClient<IReCaptchaService, ReCaptchaService>();

            // những service sử dụng AddSingleton khởi tạo từ đầu
            //services.AddSingleton<DapperContext>();

            // những service sử dụng AddTransient khởi tạo khi đc gọi
            services.AddTransient<IRoleRepository, RoleRepository>(provider =>{ return new RoleRepository(connectionString); });
            services.AddTransient<IUserRepository, UserRepository>(provider =>{ return new UserRepository(connectionString); });
            services.AddTransient<ITokenRepository, TokenRepository>(provider =>{ return new TokenRepository(connectionString); });
            services.AddTransient<ILogRepository, LogRepository>(provider => { return new LogRepository(connectionString); });
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IAuthService, AuthService>(provider => {
                var reCaptchaService = provider.GetRequiredService<IReCaptchaService>();
                var userRepository = provider.GetRequiredService<IUserRepository>();
                var tokenService = provider.GetRequiredService<ITokenService>();
                var tokenRepository = provider.GetRequiredService<ITokenRepository>();
                return new AuthService(reCaptchaService, userRepository, tokenService, tokenRepository, connectionString); 
            });

            // những service sử dụng AddScoped 

        }
    }
}
