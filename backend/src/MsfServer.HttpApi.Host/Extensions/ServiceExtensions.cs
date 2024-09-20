using MsfServer.Application.Contracts.Roles.RoleDto;
using MsfServer.Application.Contracts.roles;
using MsfServer.Application.Contracts.Users.UserDto;
using MsfServer.Application.Contracts.Users;
using MsfServer.Domain.Shared.Responses;
using MsfServer.Application.Repositorys;

namespace MsfServer.HttpApi.Host.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddCustomServices(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<ResponseObject<RoleOutput>>(provider =>
            {
                return new ResponseObject<RoleOutput>();
            });

            services.AddScoped<ResponseObject<UserOutput>>(provider =>
            {
                return new ResponseObject<UserOutput>();
            });

            services.AddScoped<IRoleRepository, RoleRepository>(provider =>
            {
                var responseObject = provider.GetRequiredService<ResponseObject<RoleOutput>>();
                return new RoleRepository(connectionString, responseObject);
            });

            services.AddScoped<IUserRepository, UserRepository>(provider =>
            {
                var responseObject = provider.GetRequiredService<ResponseObject<UserOutput>>();
                return new UserRepository(connectionString, responseObject);
            });
        }
    }
}
