
using Dapper;
using Microsoft.AspNetCore.Http;
using MsfServer.Application.Contracts.Permission;
using MsfServer.Application.Contracts.Permission.Dto;
using MsfServer.Application.Dapper;
using MsfServer.Domain.Shared.PagedResults;
using MsfServer.Domain.Shared.Responses;
using Newtonsoft.Json;
using System.Data;

namespace MsfServer.Application.Repositories
{
    public class PermissionRepository(string connectionString) : IPermissionRepository
    {
        private readonly string _connectionString = connectionString;
        public async Task<ResponseText> CreatePermissionAsync(PermissionInput input)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var perJson = new PermissionJson
            {
                Description = string.IsNullOrEmpty(input.Description) ? $"Đây là mô tả của {input.Name}" : input.Description,
                PermissionName = input.PermissionName,
                GroupName = input.PermissionName!.Split('.')[0],
                Name = input.Name,
            };
            // Chuyển đổi dữ liệu đầu vào thành JSON
            var permissionJson = JsonConvert.SerializeObject(perJson);
            var result = await connection.QuerySingleOrDefaultAsync<ResponseText>(
                "Permission_Create", new { PermissionJson = permissionJson }, commandType: CommandType.StoredProcedure);

            return ResponseText.ResponseSuccess("Thêm thành công.", StatusCodes.Status201Created);
        }

        public async Task<ResponseText> DeletePermissionAsync(int id)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var result = await connection.QuerySingleOrDefaultAsync<ResponseText>(
                "Permission_Delete", new { Id = id }, commandType: CommandType.StoredProcedure);

            return ResponseText.ResponseSuccess("Xóa thành công.", StatusCodes.Status204NoContent);
        }

        public async Task<ResponseObject<PermissionResponse>> GetPermissionByIdAsync(int id)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var permission = await connection.QuerySingleOrDefaultAsync<PermissionResponse>(
            "Permission_GetById", new { Id = id }, commandType: CommandType.StoredProcedure);

            return ResponseObject<PermissionResponse>.CreateResponse("Lấy dữ liệu thành công.", permission!);
        }

        public async Task<ResponseObject<PagedResult<PermissionResponse>>> GetPermissionsAsync(int page, int limit)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            using var multi = await connection.QueryMultipleAsync(
                 "Permission_GetAll",
                 new { Page = page, Limit = limit },
                 commandType: CommandType.StoredProcedure);

            var permissions = await multi.ReadAsync<PermissionResponse>();
            var firstRole = permissions.FirstOrDefault();

            var pagedResult = new PagedResult<PermissionResponse>
            {
                TotalRecords = firstRole?.Total ?? 0,
                Page = page,
                Limit = limit,
                Data = permissions.ToList() ?? []
            };

            return ResponseObject<PagedResult<PermissionResponse>>.CreateResponse("Lấy dữ liệu thành công.", pagedResult);
        }

        public async Task<ResponseText> UpdatePermissionAsync(PermissionInput input, int id)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var perJson = new PermissionJson
            {
                Description = string.IsNullOrEmpty(input.Description) ? $"Đây là mô tả của {input.Name}" : input.Description,
                PermissionName = input.PermissionName,
                GroupName = input.PermissionName!.Split('.')[0],
                Name = input.Name,
            };
            // Chuyển đổi dữ liệu đầu vào thành JSON
            var permissionJson = JsonConvert.SerializeObject(perJson);
            var result = await connection.QuerySingleOrDefaultAsync<ResponseText>(
                "Permission_Update", new { PermissionJson = permissionJson, Id = id }, commandType: CommandType.StoredProcedure);

            return ResponseText.ResponseSuccess("Sửa thành công.", StatusCodes.Status204NoContent);
        }
    }
}
