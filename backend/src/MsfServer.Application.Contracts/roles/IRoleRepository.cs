using MsfServer.Application.Contracts.Roles.RoleDto;
using MsfServer.Application.Contracts.Users.UserDto;
using MsfServer.Application.Page;
using MsfServer.Domain.roles;

namespace MsfServer.Application.Contracts.roles
{
    public interface IRoleRepository
    {
        Task<PagedResult<RoleOutput>> GetRolesAsync(int page, int limit);
        Task<RoleOutput> GetRoleByIdAsync(int id);
        Task<int> CreateRoleAsync(RoleInput input);
        Task<int> UpdateRoleAsync(RoleInput input, int id);
        Task<int> DeleteRoleAsync(int id);
    }
}
