using Microsoft.Extensions.DependencyInjection;

namespace MsfServer.HttpApi.Host.Extensions
{
    public static class AuthorizationServiceExtensions
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                .AddPolicy("AdminPolicy", policy => policy.RequireRole("admin"))
                .AddPolicy("UserPolicy", policy => policy.RequireRole("user"));

            return services;
        }
    }
}