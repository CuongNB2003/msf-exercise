using Dapper;
using MsfServer.Domain.roles;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using MsfServer.Application.Contracts.roles;
using MsfServer.Application.Contracts.Roles.RoleDto;
using MsfServer.Application.Page;
using MsfServer.Application.Contracts.Users.UserDto;
using System.Data;

namespace MsfServer.Application
{
    public class RoleRepository : IRoleRepository
    {
        private readonly string _connectionString;

        public RoleRepository(string connectionString) => _connectionString = connectionString;

        //lấy tất cả role
        public async Task<PagedResult<RoleOutput>> GetRolesAsync(int page, int limit)
        {
            using var connection = new SqlConnection(_connectionString);
            var offset = (page - 1) * limit;

            using var multi = await connection.QueryMultipleAsync(
                "GetPagedRoles",
                new { Offset = offset, PageSize = limit },
                commandType: CommandType.StoredProcedure);

            var totalRecords = await multi.ReadSingleAsync<int>();
            var roles = await multi.ReadAsync<RoleOutput>();

            return new PagedResult<RoleOutput>
            {
                TotalRecords = totalRecords,
                PageNumber = page,
                PageSize = limit,
                Data = roles.ToList()
            };
        }

        //lấy role theo id
        public async Task<RoleOutput> GetRoleByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Roles WHERE Id = @Id";
            var role = await connection.QuerySingleOrDefaultAsync<RoleOutput>(sql, new { Id = id });
            return role;
        }
        
        //tạo role
        public async Task<int> CreateRoleAsync(RoleInput input)
        {
            using var connection = new SqlConnection(_connectionString);
            var checkSql = "SELECT COUNT(1) FROM Roles WHERE Name = @Name";
            var exists = await connection.ExecuteScalarAsync<int>(checkSql, new { input.Name });

            if (exists > 0)
            {
                return -1;
            }
            var sql = "INSERT INTO Roles (Name) VALUES (@Name)";
            var result = await connection.ExecuteAsync(sql, input);
            return result;
        }
        
        //sửa role
        public async Task<int> UpdateRoleAsync(RoleInput role, int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var checkSql = "SELECT COUNT(1) FROM Roles WHERE Name = @Name AND Id != @Id";
            var exists = await connection.ExecuteScalarAsync<int>(checkSql, new { role.Name, Id = id });
            if (exists > 0)
            {
                return -1;
            }
            var updateSql = "UPDATE Roles SET Name = @Name WHERE Id = @Id";
            var result = await connection.ExecuteAsync(updateSql, new { role.Name, Id = id });

            return result;
        }
        //xóa role
        public async Task<int> DeleteRoleAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM Roles WHERE Id = @Id";
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            return result;
        }
    }

}
