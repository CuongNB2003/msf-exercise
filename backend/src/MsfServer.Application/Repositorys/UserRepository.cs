using MsfServer.Application.Contracts.Users;
using Dapper;
using System.Data;
using MsfServer.Application.Page;
using MsfServer.Domain.Security;
using MsfServer.Domain.Shared.Responses;
using Microsoft.AspNetCore.Http;
using MsfServer.Domain.Shared.Exceptions;
using MsfServer.Application.Database;
using MsfServer.Application.Contracts.Users.UserDtos;
using MsfServer.Application.Contracts.Roles.RoleDtos;

namespace MsfServer.Application.Repositorys
{
    public class UserRepository(string connectionString, ResponseObject<UserResultDto> responseUser) : IUserRepository
    {
        private readonly string _connectionString = connectionString;
        private readonly ResponseObject<UserResultDto> _responseUser = responseUser;

        // thêm user
        public async Task<ResponseText> CreateUserAsync(UserInput input)
        {
            //check emmail
            if (await CheckEmailExistsAsync(input.Email))
            {
                throw new CustomException(StatusCodes.Status409Conflict, "Email đã tồn tại.");
            }

            // tạo data
            byte[] salt = PasswordHashed.GenerateSalt();
            string hashedPassword = PasswordHashed.HashPassword("111111", salt);
            var user = new UserDto
            {
                Name = input.Name,
                Email = input.Email,
                Password = hashedPassword,
                RoleId = input.RoleId,
                Avatar = input.Avatar,
                Salt = Convert.ToBase64String(salt)
            };
            // add user
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            var sql = @"
                    INSERT INTO Users (Name, Email, Password, RoleId, Avatar, Salt)
                    VALUES (@Name, @Email, @Password, @RoleId, @Avatar, @Salt)";
            var result = await connection.ExecuteAsync(sql, new
            {
                user.Name,
                user.Email,
                user.Password,
                user.RoleId,
                user.Avatar,
                user.Salt
            });
            return ResponseText.ResponseSuccess("Thêm thành công", StatusCodes.Status201Created);
        }

        // sửa user
        public async Task<ResponseText> UpdateUserAsync(UserInput input, int id)
        {
            var user = await GetUserByIdAsync(id);
            // Kiểm tra email mới có trùng với email hiện tại không
            if (!string.Equals(user?.Data?.Email, input.Email, StringComparison.OrdinalIgnoreCase))
            {
                if (await CheckEmailExistsAsync(input.Email))
                {
                    throw new CustomException(StatusCodes.Status409Conflict, "Email đã tồn tại.");
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
            await GetUserByIdAsync(id);
            //xóa user
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            var deleteSql = "DELETE FROM Users WHERE Id = @Id";
            var result = await connection.ExecuteAsync(deleteSql, new { Id = id });
            return ResponseText.ResponseSuccess("Xóa thành công.", StatusCodes.Status204NoContent);
        }

        // lấy user theo id
        public async Task<ResponseObject<UserResultDto>> GetUserByIdAsync(int id)
        {
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            var sql = @"
                    SELECT * FROM Users WHERE Id = @Id;
                    SELECT * FROM Roles WHERE Id = (SELECT RoleId FROM Users WHERE Id = @Id);";

            using var multi = await connection.QueryMultipleAsync(sql, new { Id = id });

            var user = await multi.ReadSingleOrDefaultAsync<UserResultDto>();
            var role = await multi.ReadSingleOrDefaultAsync<RoleResultDto>();

            if (user == null)
            {
                throw new CustomException(StatusCodes.Status404NotFound, "Không tìm thấy User.");
            }
            if (role == null)
            {
                throw new CustomException(StatusCodes.Status404NotFound, "Không tìm thấy Role.");
            }

            user.Role = role;

            return _responseUser.ResponseSuccess("Lấy dữ liệu thành công.", user);
        }

        // lấy tất cả user
        public async Task<ResponseObject<PagedResult<UserResultDto>>> GetUsersAsync(int page, int limit)
        {
            if (page <= 0 || limit <= 0)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Bạn cần phải truyền vào page và limit.");
            }

            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            var offset = (page - 1) * limit;

            using var multi = await connection.QueryMultipleAsync(
                "GetPagedUsers",
                new { Offset = offset, PageSize = limit },
                commandType: CommandType.StoredProcedure);

            var totalRecords = await multi.ReadSingleAsync<int>();
            var userRoleData = await multi.ReadAsync<dynamic>();

            var users = userRoleData.Select(ur => new UserResultDto
            {
                Id = ur.Id,
                Name = ur.Name,
                Email = ur.Email,
                RoleId = ur.RoleId,
                Avatar = ur.Avatar,
                Role = new RoleResultDto
                {
                    Id = ur.RoleId,
                    Name = ur.RoleName
                }
            }).ToList();

            var pagedResult = new PagedResult<UserResultDto>
            {
                TotalRecords = totalRecords,
                Page = page,
                Limit = limit,
                Data = users
            };

            return new ResponseObject<PagedResult<UserResultDto>>(StatusCodes.Status200OK, "Lấy dữ liệu thành công.", pagedResult);
        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            var sql = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
            var count = await connection.ExecuteScalarAsync<int>(sql, new { Email = email });
            return count > 0;
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            var sql = @"
            SELECT * FROM Users WHERE Email = @Email;
            SELECT * FROM Roles WHERE Id = (SELECT RoleId FROM Users WHERE Email = @Email);";

            using var multi = await connection.QueryMultipleAsync(sql, new { Email = email });

            var user = await multi.ReadSingleOrDefaultAsync<UserDto>() ?? throw new CustomException(StatusCodes.Status404NotFound, "Email chưa đúng.");
            var role = await multi.ReadSingleOrDefaultAsync<RoleResultDto>();
            user.Role = role ?? throw new CustomException(StatusCodes.Status404NotFound, "Role không tồn tại.");

            return user;
        }

        public async Task<UserDto> GetUserAsync(int id)
        {
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            var sql = @"
            SELECT * FROM Users WHERE Id = @Id;
            SELECT * FROM Roles WHERE Id = (SELECT RoleId FROM Users WHERE Id = @Id);";

            using var multi = await connection.QueryMultipleAsync(sql, new { Id = id });

            var user = await multi.ReadSingleOrDefaultAsync<UserDto>() ?? throw new CustomException(StatusCodes.Status404NotFound, "Email chưa đúng.");
            var role = await multi.ReadSingleOrDefaultAsync<RoleResultDto>();
            user.Role = role ?? throw new CustomException(StatusCodes.Status404NotFound, "Role không tồn tại.");

            return user;
        }
    }

    }

