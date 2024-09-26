using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authorization;

namespace MsfServer.HttpApi.Host.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                // định nghĩa bảo mật JWT
                c.AddSecurityDefinition("Token", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header. Example: \"Authorization: {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Token"
                });
                // lọc ra những api có [Authorize] thì thêm ổ khóa
                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            return services;
        }

        public class SecurityRequirementsOperationFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                //check xem controller nào có Authorize
                var hasAuthorize = context.MethodInfo.DeclaringType!.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                                   context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();
                //nếu có Authorize thì tạo ổ khóa cho nó
                if (hasAuthorize)
                {
                    operation.Security =
                    [
                        new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Token"
                                    }
                                },
                                new List<string>()
                            }
                        }
                    ];
                }
            }
        }

    }
}
