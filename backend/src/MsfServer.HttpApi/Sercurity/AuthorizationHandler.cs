using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using MsfServer.Domain.Shared.Exceptions;
using System.Security.Claims;

namespace MsfServer.HttpApi.Sercurity
{
    public class AuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            // Kiểm tra xem người dùng đã xác thực chưa
            if (!context.User.Identity!.IsAuthenticated)
            {
                context.Fail(); // Từ chối nếu người dùng không xác thực
                return Task.CompletedTask;
            }

            if (context.Resource is HttpContext httpContext)
            {
                var endpoint = httpContext.GetEndpoint();

                // Lấy thông tin quyền từ attribute
                var authorizePermission = endpoint!.Metadata.GetMetadata<AuthorizePermissionAttribute>();
                if (authorizePermission != null)
                {
                    var requiredPermission = authorizePermission.Permission;

                    // Lấy danh sách permissions từ claims
                    var roleIds = context.User.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value)
                        .ToList();

                    // Kiểm tra xem user có ít nhất 1 permission trong danh sách yêu cầu
                    if (roleIds.Contains(requiredPermission))
                    {
                        context.Succeed(requirement); // Người dùng có quyền
                    }
                    else
                    {
                        context.Fail(); // Người dùng không có quyền
                    }
                }
                else
                {
                    context.Succeed(requirement); // Nếu không có yêu cầu quyền, cho phép mặc định
                }
            }

            return Task.CompletedTask;
        }
    }
}
