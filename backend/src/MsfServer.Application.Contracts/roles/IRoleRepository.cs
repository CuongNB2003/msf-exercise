using MsfServer.Application.Contracts.Roles.RoleDto;
using MsfServer.Domain.roles;

namespace MsfServer.Application.Contracts.roles
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetRolesAsync();
        Task<Role> GetRoleByIdAsync(int id);
        Task<int> CreateRoleAsync(RoleInput input);
        Task<int> UpdateRoleAsync(Role input);
        Task<int> DeleteRoleAsync(int id);
    }
}
