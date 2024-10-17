
using MsfServer.HttpApi.Sercurity;

namespace MsfServer.HttpApi.Host.Extensions
{
    public static class AuthorizationServiceExtensions
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("PermissionPolicy", policy =>
                    policy.Requirements.Add(new PermissionRequirement("")));
            });

            return services;
        }
    }
}