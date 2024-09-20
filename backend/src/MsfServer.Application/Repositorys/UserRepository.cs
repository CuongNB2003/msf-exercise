using MsfServer.Application.Contracts.Users;
using Dapper;
using System.Data.SqlClient;
using MsfServer.Application.Contracts.Users.UserDto;
using System.Data;
using MsfServer.Application.Contracts.Roles.RoleDto;
using MsfServer.Application.Page;
using MsfServer.Domain.users;
using MsfServer.Domain.Security;
using MsfServer.Domain.Shared.Responses;
using Microsoft.AspNetCore.Http;
using MsfServer.Domain.Shared.Exceptions;
using MsfServer.Application.Database;

namespace MsfServer.Application.Repositorys
{
    public class UserRepository(string connectionString, ResponseObject<UserOutput> responseUser) : IUserRepository
    {
        private readonly string _connectionString = connectionString;
        private readonly ResponseObject<UserOutput> _responseUser = responseUser;

        // thêm user
        public async Task<ResponseText> CreateUserAsync(UserInput input)
        {
            //check emmail
            if (await CheckEmailExistsAsync(input.Email))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Email đã tồn tại", "Create User");
            }

            // tạo data
            byte[] salt = PasswordHashed.GenerateSalt();
            string hashedPassword = PasswordHashed.HashPassword("111111", salt);
            var user = new User
            {
                Name = input.Name,
                Email = input.Email,
                Password = hashedPassword,
                RoleId = input.RoleId,
                Avatar = input.Avatar
            };
            // add user
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            var sql = @"
                    INSERT INTO Users (Name, Email, Password, RoleId, Avatar)
                    VALUES (@Name, @Email, @Password, @RoleId, @Avatar)";
            var result = await connection.ExecuteAsync(sql, new
            {
                user.Name,
                user.Email,
                user.Password,
                user.RoleId,
                user.Avatar
            });
            return ResponseText.ResponseSuccess("Thêm thành công", StatusCodes.Status201Created);
        }

        // sửa user
        public async Task<ResponseText> UpdateUserAsync(UserInput input, int id)
        {
            if (!await CheckUserExistsAsync(id))
            {
                throw new CustomException(StatusCodes.Status404NotFound, "User không tồn tại.", "Update User");
            }

            var existingEmail = await GetUserEmailByIdAsync(id);

            // Kiểm tra email mới có trùng với email hiện tại không
            if (!string.Equals(existingEmail, input.Email, StringComparison.OrdinalIgnoreCase))
            {
                if (await CheckEmailExistsAsync(input.Email))
                {
                    throw new CustomException(StatusCodes.Status400BadRequest, "Email đã tồn tại", "Update User");
                }
            }
            // cập nhật
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            var updateSql = @"
            UPDATE Users
            SET Name = @Name, Email = @Email, RoleId = @RoleId, Avatar = @Avatar
            WHERE Id = @Id";
            var result = await connection.ExecuteAsync(updateSql, new { input.Name, input.Email, input.RoleId, input.Avatar, Id = id });
            return ResponseText.ResponseSuccess("Sửa thành công.", StatusCodes.Status204NoContent);
        }

        // xóa user
        public async Task<ResponseText> DeleteUserAsync(int id)
        {
            //check user
            if (!await CheckUserExistsAsync(id))
            {
                throw new CustomException(StatusCodes.Status404NotFound, "User không tồn tại.", "Delete User");
            }
            //xóa user
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            var deleteSql = "DELETE FROM Users WHERE Id = @Id";
            var result = await connection.ExecuteAsync(deleteSql, new { Id = id });
            return ResponseText.ResponseSuccess("Xóa thành công.", StatusCodes.Status204NoContent);
        }

        // lấy user theo id
        public async Task<ResponseObject<UserOutput>> GetUserByIdAsync(int id)
        {
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            var sql = @"
                    SELECT * FROM Users WHERE Id = @Id;
                    SELECT * FROM Roles WHERE Id = (SELECT RoleId FROM Users WHERE Id = @Id);";

            using var multi = await connection.QueryMultipleAsync(sql, new { Id = id });

            var user = await multi.ReadSingleOrDefaultAsync<UserOutput>();
            var role = await multi.ReadSingleOrDefaultAsync<RoleOutput>();

            if (user == null)
            {
                throw new CustomException(StatusCodes.Status404NotFound, "Không tìm thấy User.", "Get User By Id");
            }
            if (role == null)
            {
                throw new CustomException(StatusCodes.Status404NotFound, "Không tìm thấy Role.", "Get User By Id");
            }

            user.Role = role;

            return _responseUser.ResponseSuccess("Lấy dữ liệu thành công.", user);
        }

        // lấy tất cả user
        public async Task<ResponseObject<PagedResult<UserOutput>>> GetUsersAsync(int page, int limit)
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

            var pagedResult = new PagedResult<UserOutput>
            {
                TotalRecords = totalRecords,
                PageNumber = page,
                PageSize = limit,
                Data = users.ToList()
            };

            return new ResponseObject<PagedResult<UserOutput>>(StatusCodes.Status200OK, "Lấy dữ liệu thành công.", pagedResult);
        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            var checkEmailSql = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
            var emailExists = await connection.ExecuteScalarAsync<int>(checkEmailSql, new { Email = email });
            return emailExists > 0;
        }

        public async Task<bool> CheckUserExistsAsync(int id)
        {
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            var checkUserSql = "SELECT COUNT(1) FROM Users WHERE Id = @Id";
            var userExists = await connection.ExecuteScalarAsync<int>(checkUserSql, new { Id = id });
            return userExists > 0;
        }

        public async Task<string> GetUserEmailByIdAsync(int id)
        {
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            var checkUserSql = "SELECT Email FROM Users WHERE Id = @Id";
            var existingEmail = await connection.QuerySingleOrDefaultAsync<string>(checkUserSql, new { Id = id });
            return existingEmail ?? throw new CustomException(StatusCodes.Status404NotFound, "User không tồn tại.", "Get User Email");
        }

    }
}

