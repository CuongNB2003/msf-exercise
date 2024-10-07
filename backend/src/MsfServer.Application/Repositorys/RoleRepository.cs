using Dapper;
using System.Data;
using Microsoft.AspNetCore.Http;
using MsfServer.Domain.Shared.Exceptions;
using MsfServer.Domain.Shared.Responses;
using MsfServer.Application.Contracts.Role;
using MsfServer.Domain.Shared.PagedResults;
using MsfServer.Application.Dapper;
using MsfServer.Application.Contracts.Role.Dto;

namespace MsfServer.Application.Repositorys
{
    public class RoleRepository(string connectionString) : IRoleRepository
    {
        private readonly string _connectionString = connectionString;

        //lấy tất cả role
        public async Task<ResponseObject<PagedResult<RoleResponse>>> GetRolesAsync(int page, int limit)
        {
            if (page <= 0 || limit <= 0)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Bạn cần phải truyền vào page và limit.");
            }

            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            //thực hiện truy vấn 
            var offset = (page - 1) * limit;
            using var multi = await connection.QueryMultipleAsync(
                 "Role_GetAll",
                 new { Offset = offset, PageSize = limit },
                 commandType: CommandType.StoredProcedure);

            var totalRecords = await multi.ReadSingleAsync<int>();
            var roles = await multi.ReadAsync<RoleResponse>();
            var pagedResult = new PagedResult<RoleResponse>
            {
                TotalRecords = totalRecords,
                Page = page,
                Limit = limit,
                Data = roles.ToList()
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
