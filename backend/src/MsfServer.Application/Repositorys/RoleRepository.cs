using Dapper;
using System.Data;
using Microsoft.AspNetCore.Http;
using MsfServer.Domain.Shared.Exceptions;
using MsfServer.Domain.Shared.Responses;
using MsfServer.Application.Contracts.Role.RoleDtos;
using MsfServer.Application.Contracts.Role;
using MsfServer.Domain.Shared.PagedResults;
using MsfServer.Application.Dapper;

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
                "SELECT * FROM Roles WHERE Id = @Id", new { Id = id });

            return role == null
                    ? throw new CustomException(StatusCodes.Status404NotFound, "Không tìm thấy Role.")
                    : ResponseObject<RoleResponse>.CreateResponse("Lấy dữ liệu thành công.", role);
        }
        //tạo role
        public async Task<ResponseText> CreateRoleAsync(RoleInput input)
        {
            input.Name = input.Name.ToLower();
            // check Role Name
            if (await CheckRoleNameExistsAsync(input.Name))
            {
                throw new CustomException(StatusCodes.Status409Conflict, "Role đã tồn tại.");
            }
            // add role
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var sql = "INSERT INTO Roles (Name) VALUES (@Name)";
            var result = await connection.ExecuteAsync(sql, input);
            return ResponseText.ResponseSuccess("Thêm thành công.", StatusCodes.Status201Created);
        }

        //sửa role
        public async Task<ResponseText> UpdateRoleAsync(RoleInput input, int id)
        {
            input.Name = input.Name.ToLower();
            // Kiểm tra xem role có tồn tại không
            await GetRoleByIdAsync(id);
            // check Role Name
            if (await CheckRoleNameExistsAsync(input.Name))
            {
                throw new CustomException(StatusCodes.Status409Conflict, "Name Role đã tồn tại.");
            }

            // Cập nhật role
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var updateSql = "UPDATE Roles SET Name = @Name, UpdatedAt = GETDATE() WHERE Id = @Id";
            var result = await connection.ExecuteAsync(updateSql, new { input.Name, Id = id });

            return ResponseText.ResponseSuccess("Sửa thành công.", StatusCodes.Status204NoContent);
        }

        //xóa role
        public async Task<ResponseText> DeleteRoleAsync(int id)
        {
            // Kiểm tra xem role có tồn tại không
            await GetRoleByIdAsync(id);
            // kiểm tra xem có user nào liên quan đến role không 
            var userCount = await GetUserCountByRoleIdAsync(id);

            if (userCount > 0)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, $"Không thể xóa Role vì có {userCount} User liên quan.");
            }

            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var sql = "DELETE FROM Roles WHERE Id = @Id";
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            return ResponseText.ResponseSuccess("Xóa thành công.", StatusCodes.Status204NoContent);
        }

        public async Task<bool> CheckRoleNameExistsAsync(string name)
        {
            name = name.ToLower();
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var sql = "SELECT COUNT(1) FROM Roles WHERE Name = @Name";
            var count = await connection.ExecuteScalarAsync<int>(sql, new { Name = name });
            return count > 0;
        }

        public async Task<int> GetUserCountByRoleIdAsync(int id)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var checkSql = "SELECT COUNT(*) FROM Users WHERE RoleId = @RoleId";
            return await connection.ExecuteScalarAsync<int>(checkSql, new { RoleId = id });
        }
    }
}
