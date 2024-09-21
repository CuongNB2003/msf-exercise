using MsfServer.Application.Contracts.roles;
using MsfServer.Application.Contracts.Users;
using MsfServer.Domain.Shared.Responses;
using MsfServer.Application.Repositorys;
using MsfServer.Application.Contracts.Roles.RoleDtos;
using MsfServer.Application.Contracts.Users.UserDtos;

namespace MsfServer.HttpApi.Host.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddCustomServices(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<ResponseObject<RoleResultDto>>(provider =>
            {
                return new ResponseObject<RoleResultDto>();
            });

            services.AddScoped<ResponseObject<UserResultDto>>(provider =>
            {
                return new ResponseObject<UserResultDto>();
            });

            services.AddScoped<IRoleRepository, RoleRepository>(provider =>
            {
                var responseObject = provider.GetRequiredService<ResponseObject<RoleResultDto>>();
                return new RoleRepository(connectionString, responseObject);
            });

            services.AddScoped<IUserRepository, UserRepository>(provider =>
            {
                var responseObject = provider.GetRequiredService<ResponseObject<UserResultDto>>();
                return new UserRepository(connectionString, responseObject);
            });
        }
    }
}
