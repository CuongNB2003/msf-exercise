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

namespace MsfServer.Application
{
    public class UserRepository(string connectionString, ResponseObject<UserOutput> responseUser) : IUserRepository
    {
        private readonly string _connectionString = connectionString;
        private readonly ResponseObject<UserOutput> _responseUser = responseUser;

        // thêm user
        public async Task<ResponseText> CreateUserAsync(UserInput input)
        {
            using var connection = new SqlConnection(_connectionString);
            // check email
            var checkEmailSql = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
            var emailExists = await connection.ExecuteScalarAsync<int>(checkEmailSql, new { input.Email });
            if (emailExists > 0)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Email đã tồn tại", "Create User");
            }
            // tạo data
            byte[] salt = PasswordHasher.GenerateSalt();
            string hashedPassword = PasswordHasher.HashPassword("111111", salt);
            var user = new User
            {
                Name = input.Name,
                Email = input.Email,
                Password = hashedPassword,
                RoleId = input.RoleId,
                Avatar = input.Avatar
            };
            // add user
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
        public async Task<ResponseText> UpdateUserAsync(UserInput user, int id)
        {
            using var connection = new SqlConnection(_connectionString);
            //check user
            var checkUserSql = "SELECT Email FROM Users WHERE Id = @Id";
            var existingEmail = await connection.QuerySingleOrDefaultAsync<string>(checkUserSql, new { Id = id }) 
                ?? throw new CustomException(StatusCodes.Status404NotFound, "User không tồn tại.", "Check User");
            // check email
            if (existingEmail != user.Email)
            {
                var checkEmailSql = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
                var emailExists = await connection.ExecuteScalarAsync<int>(checkEmailSql, new { Email = user.Email });

                if (emailExists > 0)
                {
                    throw new CustomException(StatusCodes.Status409Conflict, "Email đã tồn tại.", "Check Email");
                }
            }
            // cập nhật
            var updateSql = @"
            UPDATE Users
            SET Name = @Name, Email = @Email, RoleId = @RoleId, Avatar = @Avatar
            WHERE Id = @Id";
            var result = await connection.ExecuteAsync(updateSql, new { user.Name, user.Email, user.RoleId, user.Avatar, Id = id });
            return ResponseText.ResponseSuccess("Sửa thành công.", StatusCodes.Status204NoContent);
        }

        // xóa user
        public async Task<ResponseText> DeleteUserAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            //check user
            var checkUserSql = "SELECT COUNT(1) FROM Users WHERE Id = @Id";
            var userExists = await connection.ExecuteScalarAsync<int>(checkUserSql, new { Id = id });
            if (userExists == 0)
            {
                throw new CustomException(StatusCodes.Status404NotFound, "User không tồn tại.", "Delete User");
            }
            //xóa user
            var deleteSql = "DELETE FROM Users WHERE Id = @Id";
            var result = await connection.ExecuteAsync(deleteSql, new { Id = id });
            return ResponseText.ResponseSuccess("Xóa thành công.", StatusCodes.Status204NoContent);
        }

        // lấy user theo id
        public async Task<ResponseObject<UserOutput>> GetUserByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                    SELECT * FROM Users WHERE Id = @Id;
                    SELECT * FROM Roles WHERE Id = (SELECT RoleId FROM Users WHERE Id = @Id);";

            using var multi = await connection.QueryMultipleAsync(sql, new { Id = id });

            var user = await multi.ReadSingleOrDefaultAsync<UserOutput>();
            var role = await multi.ReadSingleOrDefaultAsync<RoleOutput>();

            if(user == null)
            {
                throw new CustomException(StatusCodes.Status404NotFound, "Không tìm thấy Role.", "Get User By Id");
            }
            if (role == null)
            {
                throw new CustomException(StatusCodes.Status404NotFound, "Không tìm thấy Role.", "Get Role User By Id");
            }

            user.Role = role;

            return _responseUser.ResponseSuccess("Lấy dữ liệu thành công.", user);
        }

        // lấy tất cả user
        public async Task<ResponseObject<PagedResult<UserOutput>>> GetUsersAsync(int page, int limit)
        {

            try
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
            catch (Exception ex)
            {
                throw new CustomException(StatusCodes.Status500InternalServerError, ex.Message, "Get All Users");
            }
        }
    }
}

