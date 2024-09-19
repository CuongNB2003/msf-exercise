using MsfServer.Application.Contracts.Users;
using MsfServer.Domain.users;
using Dapper;
using System.Data.SqlClient;
using MsfServer.Domain.roles;
using MsfServer.Application.Contracts.Users.UserDto;

namespace MsfServer.Application
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString) => _connectionString = connectionString;

        // thêm user
        public async Task<int> CreateUserAsync(UserInput user)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                    INSERT INTO Users (Name, Email, Password, RoleId, Avatar)
                    VALUES (@Name, @Email, @Password, @RoleId, @Avatar)";
            return await connection.ExecuteAsync(sql, user);
        }
        // xóa user
        public async Task<int> DeleteUserAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM Users WHERE Id = @Id";
            return await connection.ExecuteAsync(sql, new { Id = id });
        }
        // lấy user theo id
        public async Task<User> GetUserByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Users WHERE Id = @Id";
            var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
            return user;
        }
        // lấy tất cả user
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Users";
            return await connection.QueryAsync<User>(sql);
        }
        // sửa user
        public async Task<int> UpdateUserAsync(User user)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                    UPDATE Users
                    SET Name = @Name, Email = @Email, Password = @Password, RoleId = @RoleId, Avatar = @Avatar
                    WHERE Id = @Id";
            return await connection.ExecuteAsync(sql, user);
        }
    }
}

