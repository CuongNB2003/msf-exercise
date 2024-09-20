using Dapper;
using System.Data.SqlClient;
using MsfServer.Application.Contracts.roles;
using MsfServer.Application.Contracts.Roles.RoleDto;
using MsfServer.Application.Page;
using System.Data;
using Microsoft.AspNetCore.Http;
using MsfServer.Domain.Shared.Exceptions;
using MsfServer.Domain.Shared.Responses;

namespace MsfServer.Application
{
    public class RoleRepository(string connectionString, ResponseObject<RoleOutput> responseRole) : IRoleRepository
    {
        private readonly string _connectionString = connectionString;
        private readonly ResponseObject<RoleOutput> _responseRole = responseRole;

        //lấy tất cả role
        public async Task<ResponseObject<PagedResult<RoleOutput>>> GetRolesAsync(int page, int limit)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
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
            catch (Exception ex)
            {
                throw new CustomException(StatusCodes.Status500InternalServerError, ex.Message, "Get All Roles");
            }
        }


        //lấy role theo id
        public async Task<ResponseObject<RoleOutput>> GetRoleByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var role = await connection.QuerySingleOrDefaultAsync<RoleOutput>(
                "SELECT * FROM Roles WHERE Id = @Id", new { Id = id });

            return role == null
                    ? throw new CustomException(StatusCodes.Status404NotFound, "Không tìm thấy Role", "Get Role By Id")
                    : _responseRole.ResponseSuccess("Lấy dữ liệu thành công", role);
        }

        //tạo role
        public async Task<ResponseText> CreateRoleAsync(RoleInput input)
        {
            using var connection = new SqlConnection(_connectionString);
            var checkSql = "SELECT COUNT(1) FROM Roles WHERE Name = @Name";
            var exists = await connection.ExecuteScalarAsync<int>(checkSql, new { input.Name });

            if (exists > 0)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Role đã tồn tại", "Create Role");
            }

            var sql = "INSERT INTO Roles (Name) VALUES (@Name)";
            var result = await connection.ExecuteAsync(sql, input);
            return ResponseText.ResponseSuccess("Thêm thành công", StatusCodes.Status201Created);
        }

        //sửa role
        public async Task<ResponseText> UpdateRoleAsync(RoleInput role, int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var checkSql = "SELECT COUNT(1) FROM Roles WHERE Name = @Name AND Id != @Id";
            var exists = await connection.ExecuteScalarAsync<int>(checkSql, new { role.Name, Id = id });
            if (exists > 0)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Name đã tồn tại", "Update Role");
            }
            var updateSql = "UPDATE Roles SET Name = @Name WHERE Id = @Id";
            var result = await connection.ExecuteAsync(updateSql, new { role.Name, Id = id });

            return ResponseText.ResponseSuccess("Sửa thành công", StatusCodes.Status204NoContent);
        }

        //xóa role
        public async Task<ResponseText> DeleteRoleAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM Roles WHERE Id = @Id";
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            return ResponseText.ResponseSuccess("Xóa thành công", StatusCodes.Status204NoContent);
        }
    }
}
