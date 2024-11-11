using MsfServer.Application.Contracts.User;
using MsfServer.Application.Services;
using MsfServer.Application.Contracts.Authentication;
using MsfServer.Application.Contracts.Role;
using MsfServer.Application.Contracts.Token;
using MsfServer.Application.Contracts.Log;
using MsfServer.Application.Contracts.ReCaptcha;
using MsfServer.Application.Repositories;
using MsfServer.Application.Contracts.Menu;
using MsfServer.Application.Contracts.Permission;
using Microsoft.AspNetCore.Authorization;
using MsfServer.HttpApi.Sercurity;
using MsfServer.Application.Contracts.Statistic;
using MsfServer.Domain.Security;

namespace MsfServer.HttpApi.Host.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddCustomServices(this IServiceCollection services, string connectionString)
        {
            // những service sử dụng AddHttpClient sử dụng api bên thứ ba
            services.AddHttpClient<IReCaptchaService, ReCaptchaService>();

            // những service sử dụng AddSingleton khởi tạo từ đầu
            //services.AddSingleton<DapperContext>(connectionString);

            // những service sử dụng AddTransient khởi tạo khi đc gọi
            services.AddTransient<IRoleRepository, RoleRepository>(provider =>{ return new RoleRepository(connectionString); });
            services.AddTransient<IUserRepository, UserRepository>(provider =>{
                var jwtSettings = provider.GetRequiredService<JwtSettings>();
                return new UserRepository(connectionString, jwtSettings); 
            });
            services.AddTransient<ITokenRepository, TokenRepository>(provider =>{ return new TokenRepository(connectionString); });
            services.AddTransient<ILogRepository, LogRepository>(provider => { return new LogRepository(connectionString); }); 
            services.AddTransient<IMenuRepository, MenuRepository>(provider => { return new MenuRepository(connectionString); });
            services.AddTransient<IPermissionRepository, PermissionRepository>(provider => { return new PermissionRepository(connectionString); });
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IStatisticService, StatisticService>(provider => { return new StatisticService(connectionString); });

            services.AddTransient<IAuthService, AuthService>(provider => {
                var reCaptchaService = provider.GetRequiredService<IReCaptchaService>();
                var userRepository = provider.GetRequiredService<IUserRepository>();
                var tokenService = provider.GetRequiredService<ITokenService>();
                var tokenRepository = provider.GetRequiredService<ITokenRepository>();
                var jwtSettings = provider.GetRequiredService<JwtSettings>();
                return new AuthService(reCaptchaService, userRepository, tokenService, tokenRepository,jwtSettings, connectionString); 
            });

            // những service sử dụng AddScoped 
            services.AddScoped<IAuthorizationHandler, AuthorizationHandler>(provider => { return new AuthorizationHandler(connectionString); });


        }
    }
}
