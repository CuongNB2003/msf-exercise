using Dapper;
using MsfServer.Domain.roles;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using MsfServer.Application.Contracts.roles;

namespace MsfServer.Application
{
    public class RoleRepository : IRoleRepository
    {
        private readonly string _connectionString;

        public RoleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Role>> GetRolesAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Roles";
                var roles = await connection.QueryAsync<Role>(sql);
                return roles;
            }
        }

        public async Task<Role> GetRoleByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Roles WHERE Id = @Id";
                var role = await connection.QuerySingleOrDefaultAsync<Role>(sql, new { Id = id });
                return role;
            }
        }

        public async Task<int> CreateRoleAsync(Role role)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "INSERT INTO Roles (Name) VALUES (@Name)";
                var result = await connection.ExecuteAsync(sql, role);
                return result;
            }
        }

        public async Task<int> UpdateRoleAsync(Role role)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "UPDATE Roles SET Name = @Name WHERE Id = @Id";
                var result = await connection.ExecuteAsync(sql, role);
                return result;
            }
        }

        public async Task<int> DeleteRoleAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "DELETE FROM Roles WHERE Id = @Id";
                var result = await connection.ExecuteAsync(sql, new { Id = id });
                return result;
            }
        }
    }

}
