using MsfServer.Domain.roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsfServer.Application.Contracts.roles
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetRolesAsync();
        Task<Role> GetRoleByIdAsync(int id);
        Task<int> CreateRoleAsync(Role role);
        Task<int> UpdateRoleAsync(Role role);
        Task<int> DeleteRoleAsync(int id);
    }
}
