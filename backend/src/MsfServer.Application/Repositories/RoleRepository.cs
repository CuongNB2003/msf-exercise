using Dapper;
using System.Data;
using Microsoft.AspNetCore.Http;
using MsfServer.Domain.Shared.Responses;
using MsfServer.Application.Contracts.Role;
using MsfServer.Domain.Shared.PagedResults;
using MsfServer.Application.Contracts.Dapper;
using MsfServer.Application.Contracts.Role.Dto;
using Newtonsoft.Json;
using MsfServer.Application.Contracts.Menu.Dto;
using MsfServer.Application.Contracts.Permission.Dto;
using MsfServer.Domain.Shared.Exceptions;

namespace MsfServer.Application.Repositories
{
    public class RoleRepository(string connectionString) : IRoleRepository
    {
        private readonly string _connectionString = connectionString;

        //lấy tất cả role
        public async Task<ResponseObject<PagedResult<RoleResponse>>> GetRolesAsync(int page, int limit)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();

            using var multi = await connection.QueryMultipleAsync(
                "Role_GetAll",
                new { Page = page, Limit = limit },
                commandType: CommandType.StoredProcedure);

            // Lấy danh sách các role
            var roles = (await multi.ReadAsync<RoleResponse>()).ToList();
            var firstRole = roles.FirstOrDefault();
            var menusData = (await multi.ReadAsync<dynamic>()).ToList();
            var permissionsData = (await multi.ReadAsync<dynamic>()).ToList();

            // Map menu và permission vào từng role
            foreach (var role in roles)
            {
                role.Menus = menusData
                    .Where(menu => menu.RoleId == role.Id)
                    .Select(menu => new MenuRoleResponse
                    {
                        Id = menu.MenuId,
                        DisplayName = menu.DisplayName,
                        Status = menu.Status,
                        Url = menu.Url,
                        IconName = menu.IconName
                    }).ToList();

                role.Permissions = permissionsData
                    .Where(permission => permission.RoleId == role.Id)
                    .Select(permission => new PermissionRoleResponse
                    {
                        Id = permission.PermissionId,
                        PermissionName = permission.PermissionName,
                        Description = permission.Description
                    }).ToList();
            }

            var pagedResult = new PagedResult<RoleResponse>
            {
                TotalRecords = firstRole?.Total ?? 0,
                Page = page,
                Limit = limit,
                Data = roles
            };

            return ResponseObject<PagedResult<RoleResponse>>.CreateResponse("Lấy dữ liệu thành công.", pagedResult);
        }

        //lấy role theo id
        public async Task<ResponseObject<RoleResponse>> GetRoleByIdAsync(int id)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();

            // Sử dụng QueryMultiple để lấy thông tin role, menus và permissions
            using var multi = await connection.QueryMultipleAsync(
                "Role_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure);

            // Lấy thông tin role
            var role = await multi.ReadSingleOrDefaultAsync<RoleResponse>()
                ?? throw new CustomException(StatusCodes.Status404NotFound, "Role không tồn tại.");

            // Lấy danh sách menus
            var menus = (await multi.ReadAsync<MenuRoleResponse>()).ToList();
            role.Menus = menus;

            // Lấy danh sách permissions
            var permissions = (await multi.ReadAsync<PermissionRoleResponse>()).ToList();
            role.Permissions = permissions;

            // Trả về kết quả
            return ResponseObject<RoleResponse>.CreateResponse("Lấy dữ liệu thành công.", role);
        }

        //tạo role
        public async Task<ResponseText> CreateRoleAsync(RoleInput input)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var roleJson = JsonConvert.SerializeObject(input);

            var result = await connection.QuerySingleOrDefaultAsync<ResponseText>(
                "Role_Create", new { RoleJson = roleJson }, commandType: CommandType.StoredProcedure);

            return ResponseText.ResponseSuccess("Thêm thành công.", StatusCodes.Status201Created);
        }

        //sửa role
        public async Task<ResponseText> UpdateRoleAsync(RoleInput input, int id)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var roleJson = JsonConvert.SerializeObject(input);
            var result = await connection.QuerySingleOrDefaultAsync<ResponseText>(
                "Role_Update", new { RoleJson = roleJson, Id = id }, commandType: CommandType.StoredProcedure);

            return ResponseText.ResponseSuccess("Sửa thành công.", StatusCodes.Status204NoContent);
        }

        //xóa role
        public async Task<ResponseText> DeleteRoleAsync(int id)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var result = await connection.QuerySingleOrDefaultAsync<ResponseText>(
                "Role_Delete", new { Id = id }, commandType: CommandType.StoredProcedure);

            return ResponseText.ResponseSuccess("Xóa thành công.", StatusCodes.Status204NoContent);
        }
    }
}
