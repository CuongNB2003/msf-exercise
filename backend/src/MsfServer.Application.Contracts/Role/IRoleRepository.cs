﻿using MsfServer.Application.Contracts.Role.RoleDtos;
using MsfServer.Domain.Shared.PagedResults;
using MsfServer.Domain.Shared.Responses;

namespace MsfServer.Application.Contracts.Role
{
    public interface IRoleRepository
    {
        // hàm trong controller 
        Task<ResponseObject<PagedResult<RoleResponse>>> GetRolesAsync(int page, int limit);
        Task<ResponseObject<RoleResponse>> GetRoleByIdAsync(int id);
        Task<ResponseText> CreateRoleAsync(RoleInput input);
        Task<ResponseText> UpdateRoleAsync(RoleInput input, int id);
        Task<ResponseText> DeleteRoleAsync(int id);
        // hàm phụ việc truy vấn
        Task<bool> CheckRoleNameExistsAsync(string name);
        Task<int> GetUserCountByRoleIdAsync(int id);
    }
}
