using Dapper;
using System.Data.SqlClient;
using MsfServer.Application.Contracts.roles;
using MsfServer.Application.Contracts.Roles.RoleDto;
using MsfServer.Application.Page;
using System.Data;
using Microsoft.AspNetCore.Http;
using MsfServer.Domain.Shared.Exceptions;
using MsfServer.Domain.Shared.Responses;
using MsfServer.Application.Database;
using MsfServer.Domain.roles;

namespace MsfServer.Application.Repositorys
{
    public class RoleRepository(string connectionString, ResponseObject<RoleOutput> responseRole) : IRoleRepository
    {
        private readonly string _connectionString = connectionString;
        private readonly ResponseObject<RoleOutput> _responseRole = responseRole;

        //lấy tất cả role
        public async Task<ResponseObject<PagedResult<RoleOutput>>> GetRolesAsync(int page, int limit)
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
                var roles = await multi.ReadAsync<RoleOutput>();

                var pagedResult = new PagedResult<RoleOutput>
                {
                    TotalRecords = totalRecords,
                    PageNumber = page,
                    PageSize = limit,
                    Data = roles.ToList()
                };

                return new ResponseObject<PagedResult<RoleOutput>>(StatusCodes.Status200OK, "Lấy dữ liệu thành công", pagedResult);
        }
        //lấy role theo id
        public async Task<ResponseObject<RoleOutput>> GetRoleByIdAsync(int id)
        {
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            //truy vấn lấy role theo id
            var role = await connection.QuerySingleOrDefaultAsync<RoleOutput>(
                "SELECT * FROM Roles WHERE Id = @Id", new { Id = id });

            return role == null
                    ? throw new CustomException(StatusCodes.Status404NotFound, "Không tìm thấy Role", "Get Role By Id")
                    : _responseRole.ResponseSuccess("Lấy dữ liệu thành công", role);
        }
        //tạo role
        public async Task<ResponseText> CreateRoleAsync(RoleInput input)
        {
            // check Role Name
            if (await CheckRoleNameExistsAsync(input.Name))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Role Name đã tồn tại", "Create Role");
            }
            // add role
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            var sql = "INSERT INTO Roles (Name) VALUES (@Name)";
            var result = await connection.ExecuteAsync(sql, input);
            return ResponseText.ResponseSuccess("Thêm thành công", StatusCodes.Status201Created);
        }
        //sửa role
        public async Task<ResponseText> UpdateRoleAsync(RoleInput input, int id)
        {
            // Kiểm tra xem role có tồn tại không
            if (!await RoleExistsAsync(id))
            {
                throw new CustomException(StatusCodes.Status404NotFound, "Role không tồn tại", "Update Role");
            }
            // check Role Name
            if (await CheckRoleNameExistsAsync(input.Name, id))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Role Name đã tồn tại", "Update Role");
            }

            // Cập nhật role
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            var updateSql = "UPDATE Roles SET Name = @Name WHERE Id = @Id";
            var result = await connection.ExecuteAsync(updateSql, new { input.Name, Id = id });

            return ResponseText.ResponseSuccess("Sửa thành công", StatusCodes.Status204NoContent);
        }
        //xóa role
        public async Task<ResponseText> DeleteRoleAsync(int id)
        {
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();

            // Kiểm tra xem role có tồn tại không
            if (!await RoleExistsAsync(id))
            {
                throw new CustomException(StatusCodes.Status404NotFound, "Role không tồn tại", "Delete Role");
            }

            var sql = "DELETE FROM Roles WHERE Id = @Id";
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            return ResponseText.ResponseSuccess("Xóa thành công", StatusCodes.Status204NoContent);
        }
        // dùng để kiểm tra role có tồn tại không
        public async Task<bool> RoleExistsAsync(int id)
        {
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();

            var sql = "SELECT COUNT(1) FROM Roles WHERE Id = @Id";
            var exists = await connection.ExecuteScalarAsync<int>(sql, new { Id = id });
            return exists > 0;
        }
        //dùng để kiểm tra name role có tồn tại không
        public async Task<bool> CheckRoleNameExistsAsync(string roleName, int? excludeId = null)
        {
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();

            var sql = excludeId.HasValue
                ? "SELECT COUNT(1) FROM Roles WHERE Name = @Name AND Id != @Id"
                : "SELECT COUNT(1) FROM Roles WHERE Name = @Name";

            var parameters = new
            {
                Name = roleName,
                Id = excludeId ?? 0 // Sử dụng giá trị mặc định nếu excludeId là null
            };

            var exists = await connection.ExecuteScalarAsync<int>(sql, parameters);
            return exists > 0;
        }

    }
}
