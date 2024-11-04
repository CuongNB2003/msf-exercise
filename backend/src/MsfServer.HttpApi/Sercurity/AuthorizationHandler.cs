using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using MsfServer.Application.Contracts.Dapper;
using System.Data;
using System.Security.Claims;
using Dapper;

namespace MsfServer.HttpApi.Sercurity
{
    public class AuthorizationHandler(string connectionString) : AuthorizationHandler<PermissionRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            // Kiểm tra xem người dùng đã xác thực chưa
            if (!context.User.Identity!.IsAuthenticated)
            {
                context.Fail(); // Từ chối nếu người dùng không xác thực
                return;
            }

            if (context.Resource is HttpContext httpContext)
            {
                var endpoint = httpContext.GetEndpoint();

                // Lấy thông tin quyền từ attribute
                var authorizePermission = endpoint!.Metadata.GetMetadata<AuthorizePermissionAttribute>();
                if (authorizePermission != null)
                {
                    var requiredPermission = authorizePermission.Permission;

                    // Lấy danh sách roleIds từ claims
                    var roleIds = context.User.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value)
                        .ToList();

                    bool isPermission = false;

                    foreach (var id in roleIds)
                    {
                        bool hasPermission = await CheckPermissionAsync(int.Parse(id), requiredPermission);

                        if (hasPermission)
                        {
                            isPermission = true;
                            break;
                        }
                    }

                    if (isPermission)
                    {
                        context.Succeed(requirement);
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
        }

        // Phương thức kiểm tra quyền của role qua stored procedure
        private async Task<bool> CheckPermissionAsync(int roleId, string permissionName)
        {
            using var dapperContext = new DapperContext(connectionString);
            using var connection = dapperContext.GetOpenConnection();

            // Gọi stored procedure và lấy kết quả
            var result = await connection.ExecuteScalarAsync<int?>(
                "Role_CheckPermission",
                new { roleId, permissionName },
                commandType: CommandType.StoredProcedure
            );

            // Trả về true nếu có quyền (result == 1), false nếu không (result == 0)
            return result.HasValue && result.Value ==1;
        }
    }
}
