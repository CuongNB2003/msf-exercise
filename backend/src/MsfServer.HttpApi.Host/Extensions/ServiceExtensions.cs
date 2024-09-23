using MsfServer.Application.Contracts.Users;
using MsfServer.Domain.Shared.Responses;
using MsfServer.Application.Repositorys;
using MsfServer.Application.Contracts.Roles.RoleDtos;
using MsfServer.Application.Contracts.Users.UserDtos;
using MsfServer.Application.Contracts.Services;
using MsfServer.Application.Services;
using MsfServer.Application.Contracts.Authentication.AuthDtos;
using MsfServer.Application.Contracts.Authentication;
using MsfServer.Application.Contracts.Role;
using MsfServer.Application.Contracts.Token;
using MsfServer.Domain.Security;

namespace MsfServer.HttpApi.Host.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddCustomServices(this IServiceCollection services, string connectionString)
        {
            // service RoleRepository
            services.AddScoped<ResponseObject<RoleResultDto>>(provider =>
            {
                return new ResponseObject<RoleResultDto>();
            });
            services.AddScoped<IRoleRepository, RoleRepository>(provider =>
            {
                var responseObject = provider.GetRequiredService<ResponseObject<RoleResultDto>>();
                return new RoleRepository(connectionString, responseObject);
            });

            // service UserRepository
            services.AddScoped<ResponseObject<UserResultDto>>(provider =>
            {
                return new ResponseObject<UserResultDto>();
            });
            services.AddScoped<IUserRepository, UserRepository>(provider =>
            {
                var responseObject = provider.GetRequiredService<ResponseObject<UserResultDto>>();
                return new UserRepository(connectionString, responseObject);
            });

            // service TokenRepository
            services.AddScoped<ITokenRepository, TokenRepository>(provider =>
            {
                return new TokenRepository(connectionString);
            });

            // service ReCaptchaService
            services.AddHttpClient<IReCaptchaService, ReCaptchaService>();

            // service TokenService
            services.AddScoped<ITokenService, TokenService>(provider =>
            {
                var tokenRepository = provider.GetRequiredService<ITokenRepository>();
                var jwtSettings = provider.GetRequiredService<JwtSettings>();
                return new TokenService(tokenRepository, jwtSettings);
            });

            // service AuthService
            services.AddScoped<ResponseObject<LoginResultDto>>(provider =>
            {
                return new ResponseObject<LoginResultDto>();
            });
            services.AddScoped<IAuthService, AuthService>(provider =>
            {
                var reCaptchaService = provider.GetRequiredService<IReCaptchaService>();
                var userRepository = provider.GetRequiredService<IUserRepository>();
                var tokenService = provider.GetRequiredService<ITokenService>();
                var response = provider.GetRequiredService<ResponseObject<LoginResultDto>>();
                return new AuthService(reCaptchaService, userRepository, response, connectionString, tokenService);
            });

        }
    }
}
