using Dapper;
using System.Data;
using Microsoft.AspNetCore.Http;
using MsfServer.Domain.Shared.Responses;
using MsfServer.Application.Contracts.Role;
using MsfServer.Domain.Shared.PagedResults;
using MsfServer.Application.Dapper;
using MsfServer.Application.Contracts.Role.Dto;

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

            var roles = await multi.ReadAsync<RoleResponse>();
            var firstRole = roles.FirstOrDefault();

            var pagedResult = new PagedResult<RoleResponse>
            {
                TotalRecords = firstRole?.TotalRole ?? 0,
                Page = page,
                Limit = limit,
                Data = roles.ToList() ?? []
            };

            return ResponseObject<PagedResult<RoleResponse>>.CreateResponse("Lấy dữ liệu thành công.", pagedResult);
        }
        //lấy role theo id
        public async Task<ResponseObject<RoleResponse>> GetRoleByIdAsync(int id)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            //truy vấn lấy role theo id
            var role = await connection.QuerySingleOrDefaultAsync<RoleResponse>(
            "Role_GetById", new { Id = id }, commandType: CommandType.StoredProcedure);

            return ResponseObject<RoleResponse>.CreateResponse("Lấy dữ liệu thành công.", role!);
        }
        //tạo role
        public async Task<ResponseText> CreateRoleAsync(RoleInput input)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var result = await connection.QuerySingleOrDefaultAsync<ResponseText>(
                "Role_Create", new { input.Name }, commandType: CommandType.StoredProcedure);

            return ResponseText.ResponseSuccess("Thêm thành công.", StatusCodes.Status201Created);
        }

        //sửa role
        public async Task<ResponseText> UpdateRoleAsync(RoleInput input, int id)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var result = await connection.QuerySingleOrDefaultAsync<ResponseText>(
                "Role_Update", new { input.Name, Id = id }, commandType: CommandType.StoredProcedure);

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
