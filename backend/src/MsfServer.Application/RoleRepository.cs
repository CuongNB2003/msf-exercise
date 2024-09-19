using Dapper;
using MsfServer.Domain.roles;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using MsfServer.Application.Contracts.roles;
using MsfServer.Application.Contracts.Roles.RoleDto;

namespace MsfServer.Application
{
    public class RoleRepository : IRoleRepository
    {
        private readonly string _connectionString;

        public RoleRepository(string connectionString) => _connectionString = connectionString;

        //lấy tất cả role
        public async Task<IEnumerable<Role>> GetRolesAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Roles";
            var roles = await connection.QueryAsync<Role>(sql);
            return roles;
        }
        //lấy role theo id
        public async Task<Role> GetRoleByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Roles WHERE Id = @Id";
            var role = await connection.QuerySingleOrDefaultAsync<Role>(sql, new { Id = id });
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
        public async Task<int> UpdateRoleAsync(Role role)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "UPDATE Roles SET Name = @Name WHERE Id = @Id";
            var result = await connection.ExecuteAsync(sql, role);
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
