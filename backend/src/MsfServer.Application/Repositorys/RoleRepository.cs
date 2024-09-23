using Dapper;
using MsfServer.Application.Page;
using System.Data;
using Microsoft.AspNetCore.Http;
using MsfServer.Domain.Shared.Exceptions;
using MsfServer.Domain.Shared.Responses;
using MsfServer.Application.Contracts.Roles.RoleDtos;
using MsfServer.Application.Database;
using MsfServer.Application.Contracts.Role;

namespace MsfServer.Application.Repositorys
{
    public class RoleRepository(string connectionString, ResponseObject<RoleResultDto> responseRole) : IRoleRepository
    {
        private readonly string _connectionString = connectionString;
        private readonly ResponseObject<RoleResultDto> _responseRole = responseRole;

        //lấy tất cả role
        public async Task<ResponseObject<PagedResult<RoleResultDto>>> GetRolesAsync(int page, int limit)
        {
                using var dbManager = new DatabaseConnectionManager(_connectionString);
                using var connection = dbManager.GetOpenConnection();
                //thực hiện truy vấn 
                var offset = (page - 1) * limit;
                using var multi = await connection.QueryMultipleAsync(
                    "GetPagedRoles",
                    new { Offset = offset, PageSize = limit },
                    commandType: CommandType.StoredProcedure);

                var totalRecords = await multi.ReadSingleAsync<int>();
                var roles = await multi.ReadAsync<RoleResultDto>();

                var pagedResult = new PagedResult<RoleResultDto>
                {
                    TotalRecords = totalRecords,
                    Page = page,
                    Limit = limit,
                    Data = roles.ToList()
                };

                return new ResponseObject<PagedResult<RoleResultDto>>(StatusCodes.Status200OK, "Lấy dữ liệu thành công.", pagedResult);
        }
        //lấy role theo id
        public async Task<ResponseObject<RoleResultDto>> GetRoleByIdAsync(int id)
        {
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            //truy vấn lấy role theo id
            var role = await connection.QuerySingleOrDefaultAsync<RoleResultDto>(
                "SELECT * FROM Roles WHERE Id = @Id", new { Id = id });

            return role == null
                    ? throw new CustomException(StatusCodes.Status404NotFound, "Không tìm thấy Role.")
                    : _responseRole.ResponseSuccess("Lấy dữ liệu thành công.", role);
        }
        //tạo role
        public async Task<ResponseText> CreateRoleAsync(RoleInputDto input)
        {
            // check Role Name
            if (await CheckRoleNameExistsAsync(input.Name))
            {
                throw new CustomException(StatusCodes.Status409Conflict, "Role đã tồn tại.");
            }
            // add role
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            var sql = "INSERT INTO Roles (Name) VALUES (@Name)";
            var result = await connection.ExecuteAsync(sql, input);
            return ResponseText.ResponseSuccess("Thêm thành công.", StatusCodes.Status201Created);
        }
        //sửa role
        public async Task<ResponseText> UpdateRoleAsync(RoleInputDto input, int id)
        {
            // Kiểm tra xem role có tồn tại không
            await GetRoleByIdAsync(id);
            // check Role Name
            if (await CheckRoleNameExistsAsync(input.Name))
            {
                throw new CustomException(StatusCodes.Status409Conflict, "Name Role đã tồn tại.");
            }

            // Cập nhật role
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            var updateSql = "UPDATE Roles SET Name = @Name WHERE Id = @Id";
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

            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            var sql = "DELETE FROM Roles WHERE Id = @Id";
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            return ResponseText.ResponseSuccess("Xóa thành công.", StatusCodes.Status204NoContent);
        }

        public async Task<bool> CheckRoleNameExistsAsync(string name)
        {
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            var sql = "SELECT COUNT(1) FROM Roles WHERE Name = @Name";
            var count = await connection.ExecuteScalarAsync<int>(sql, new { Name = name });
            return count > 0;
        }

        public async Task<int> GetUserCountByRoleIdAsync(int id)
        {
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            var checkSql = "SELECT COUNT(*) FROM Users WHERE RoleId = @RoleId";
            return await connection.ExecuteScalarAsync<int>(checkSql, new { RoleId = id });
        }
    }
}
