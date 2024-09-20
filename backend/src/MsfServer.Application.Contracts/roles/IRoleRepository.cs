using MsfServer.Application.Contracts.Roles.RoleDto;
using MsfServer.Application.Page;
using MsfServer.Domain.Shared.Responses;

namespace MsfServer.Application.Contracts.roles
{
    public interface IRoleRepository
    {
        Task<ResponseObject<PagedResult<RoleOutput>>> GetRolesAsync(int page, int limit);
        Task<ResponseObject<RoleOutput>> GetRoleByIdAsync(int id);
        Task<ResponseText> CreateRoleAsync(RoleInput input);
        Task<ResponseText> UpdateRoleAsync(RoleInput input, int id);
        Task<ResponseText> DeleteRoleAsync(int id);
        Task<bool> RoleExistsAsync(int id);
        Task<bool> CheckRoleNameExistsAsync(string roleName, int? excludeId = null);
    }
}
