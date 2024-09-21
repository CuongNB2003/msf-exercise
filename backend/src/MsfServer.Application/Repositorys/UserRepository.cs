using MsfServer.Application.Contracts.Users;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using MsfServer.Application.Page;
using MsfServer.Domain.users;
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
            await GetUByEmailAsync(input.Email);

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
            var user = await GetUByIdAsync(id);
            // Kiểm tra email mới có trùng với email hiện tại không
            if (!string.Equals(user.Email, input.Email, StringComparison.OrdinalIgnoreCase))
            {
                await GetUByEmailAsync(input.Email);
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
            await GetUByIdAsync(id);
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
            using var connection = new SqlConnection(_connectionString);
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
                PageNumber = page,
                PageSize = limit,
                Data = users.ToList()
            };

            return new ResponseObject<PagedResult<UserResultDto>>(StatusCodes.Status200OK, "Lấy dữ liệu thành công.", pagedResult);
        }

        public async Task<UserResultDto> GetUByEmailAsync(string email)
        {
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            var sql = "SELECT * FROM Users WHERE Email = @Email";
            var user = await connection.ExecuteScalarAsync<UserResultDto>(sql, new { Email = email });
            return user ?? throw new CustomException(StatusCodes.Status404NotFound, "User không tồn tại.");
        }

        public async Task<UserResultDto> GetUByIdAsync(int id)
        {
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            var sql = "SELECT Email FROM Users WHERE Id = @Id";
            var user = await connection.QuerySingleOrDefaultAsync<UserResultDto>(sql, new { Id = id });
            return user ?? throw new CustomException(StatusCodes.Status404NotFound, "User không tồn tại.");
        }
    }
}

