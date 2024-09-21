using MsfServer.Application.Contracts.Roles.RoleDtos;
using MsfServer.Application.Page;
using MsfServer.Domain.Shared.Responses;

namespace MsfServer.Application.Contracts.roles
{
    public interface IRoleRepository
    {
        // hàm trong controller 
        Task<ResponseObject<PagedResult<RoleResultDto>>> GetRolesAsync(int page, int limit);
        Task<ResponseObject<RoleResultDto>> GetRoleByIdAsync(int id);
        Task<ResponseText> CreateRoleAsync(RoleInputDto input);
        Task<ResponseText> UpdateRoleAsync(RoleInputDto input, int id);
        Task<ResponseText> DeleteRoleAsync(int id);
        // hàm phụ việc truy vấn
        Task<RoleResultDto> GetRByIdAsyns(int id);
        Task<RoleResultDto> GetRByNameAsyns(string name);
    }
}
