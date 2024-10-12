
using MsfServer.Application.Contracts.Menu.Dto;
using MsfServer.Application.Contracts.Permission.Dto;
using MsfServer.Domain.Shared.PagedResults;
using MsfServer.Domain.Shared.Responses;

namespace MsfServer.Application.Contracts.Permission
{
    public interface IPermissionRepository
    {
        Task<ResponseObject<PagedResult<PermissionResponse>>> GetPermissionsAsync(int page, int limit);
        Task<ResponseObject<PermissionResponse>> GetPermissionByIdAsync(int id);
        Task<ResponseText> CreatePermissionAsync(PermissionInput input);
        Task<ResponseText> UpdatePermissionAsync(PermissionInput input, int id);
        Task<ResponseText> DeletePermissionAsync(int id);
    }
}
