using MsfServer.Application.Contracts.Users;
using MsfServer.Domain.users;
using Dapper;
using System.Data.SqlClient;
using MsfServer.Domain.roles;
using MsfServer.Application.Contracts.Users.UserDto;
using System.Data;
using MsfServer.Application.Contracts.Roles.RoleDto;
using MsfServer.Application.Page;
using System.Collections.Generic;

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

            var checkEmailSql = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
            var emailExists = await connection.ExecuteScalarAsync<int>(checkEmailSql, new { user.Email });

            if (emailExists > 0)
            {
                throw new InvalidOperationException("Email already exists.");
            }

            var sql = @"
                    INSERT INTO Users (Name, Email, Password, RoleId, Avatar)
                    VALUES (@Name, @Email, @Password, @RoleId, @Avatar)";
            return await connection.ExecuteAsync(sql, user);
        }

        // sửa user
        public async Task<int> UpdateUserAsync(UserInput user)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                    UPDATE Users
                    SET Name = @Name, Email = @Email, Password = @Password, RoleId = @RoleId, Avatar = @Avatar
                    WHERE Id = @Id";
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
        public async Task<UserOutput> GetUserByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                    SELECT * FROM Users WHERE Id = @Id;
                    SELECT * FROM Roles WHERE Id = (SELECT RoleId FROM Users WHERE Id = @Id);";

            using var multi = await connection.QueryMultipleAsync(sql, new { Id = id });

            var user = await multi.ReadSingleOrDefaultAsync<UserOutput>();
            var role = await multi.ReadSingleOrDefaultAsync<RoleOutput>();

            if (user != null)
            {
                user.Role = role;
            }

            return user;
        }

        // lấy tất cả user
        public async Task<PagedResult<UserOutput>> GetUsersAsync(int page, int limit)
        {
            using var connection = new SqlConnection(_connectionString);
            var offset = (page - 1) * limit;

            using var multi = await connection.QueryMultipleAsync(
                "GetPagedUsers",
                new { Offset = offset, PageSize = limit },
                commandType: CommandType.StoredProcedure);

            var totalRecords = await multi.ReadSingleAsync<int>();
            var userRoleData = await multi.ReadAsync<dynamic>();

            var users = userRoleData.Select(ur => new UserOutput
            {
                Id = ur.Id,
                Name = ur.Name,
                Email = ur.Email,
                RoleId = ur.RoleId,
                Avatar = ur.Avatar,
                Role = new RoleOutput
                {
                    Id = ur.RoleId,
                    Name = ur.RoleName
                }
            }).ToList();

            return new PagedResult<UserOutput>
            {
                TotalRecords = totalRecords,
                PageNumber = page,
                PageSize = limit,
                Data = users.ToList()
            };
        }

    }
}

