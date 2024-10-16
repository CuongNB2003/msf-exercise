using MsfServer.Application.Contracts.User;
using Dapper;
using System.Data;
using MsfServer.Domain.Security;
using MsfServer.Domain.Shared.Responses;
using Microsoft.AspNetCore.Http;
using MsfServer.Domain.Shared.Exceptions;
using MsfServer.Application.Contracts.User.Dto;
using MsfServer.Domain.Shared.PagedResults;
using MsfServer.Application.Dapper;
using Newtonsoft.Json;
using MsfServer.Application.Contracts.Role.Dto;

namespace MsfServer.Application.Repositories
{
    public class UserRepository(string connectionString) : IUserRepository
    {
        private readonly string _connectionString = connectionString;

        // thêm user
        public async Task<ResponseText> CreateUserAsync(CreateUserInput input)
        {
            // Tạo dữ liệu
            byte[] salt = PasswordHashed.GenerateSalt();
            string hashedPassword = PasswordHashed.HashPassword("111111", salt);
            var user = UserDto.CreateUserAdminDto(input.Email, hashedPassword, input.Avatar, salt, input.RoleIds);
            var userJson = JsonConvert.SerializeObject(user);
            // Thêm người dùng
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var userId = await connection.ExecuteAsync(
                "User_Create",
                 new { UserJson = userJson },
                 commandType: CommandType.StoredProcedure
            );

            return ResponseText.ResponseSuccess("Thêm thành công", StatusCodes.Status201Created);
        }

        // sửa user
        public async Task<ResponseText> UpdateUserAsync(UpdateUserInput input, int id)
        {
            // Chuyển đổi dữ liệu đầu vào thành JSON
            var userJson = JsonConvert.SerializeObject(input);
            // Cập nhật
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var result = await connection.ExecuteAsync(
                "User_Update",
                new { UserJson = userJson, Id = id },
                commandType: CommandType.StoredProcedure
            );
            return ResponseText.ResponseSuccess("Sửa thành công.", StatusCodes.Status204NoContent);
        }

        // xóa user
        public async Task<ResponseText> DeleteUserAsync(int id)
        {
            // Xóa người dùng
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var result = await connection.ExecuteAsync(
                "User_Delete",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
            return ResponseText.ResponseSuccess("Xóa thành công.", StatusCodes.Status204NoContent);
        }

        // lấy user theo id
        public async Task<ResponseObject<UserResponse>> GetUserByIdAsync(int id)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();

            // Thực hiện truy vấn hai lần
            using var multi = await connection.QueryMultipleAsync(
                "User_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );

            // Đọc thông tin người dùng
            var user = await multi.ReadSingleOrDefaultAsync<UserResponse>()
                        ?? throw new CustomException(StatusCodes.Status404NotFound, "Người dùng không tồn tại.");

            // Đọc danh sách vai trò
            var roles = await multi.ReadAsync<RoleDto>();
            user.Roles = roles.ToList(); // Gán danh sách vai trò cho user

            return ResponseObject<UserResponse>.CreateResponse("Lấy dữ liệu thành công.", user);
        }

        // lấy tất cả user
        public async Task<ResponseObject<PagedResult<UserResponse>>> GetUsersAsync(int page, int limit)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            using var multi = await connection.QueryMultipleAsync(
                "User_GetAll",
                new { Page = page, Limit = limit },
                commandType: CommandType.StoredProcedure);

            // Lấy dữ liệu người dùng
            var usersData = (await multi.ReadAsync<UserResponse>()).ToList();

            // Lấy dữ liệu vai trò cho từng người dùng
            var userRoles = (await multi.ReadAsync<dynamic>()).ToList();

            // Map vai trò vào từng người dùng
            foreach (var user in usersData)
            {
                user.Roles = userRoles
                    .Where(role => role.UserId == user.Id) // Lọc vai trò theo UserId
                    .Select(role => new RoleDto
                    {
                        Id = role.Id,
                        Name = role.Name
                    }).ToList();
            }

            var firstUser = usersData.FirstOrDefault();

            var pagedResult = new PagedResult<UserResponse>
            {
                TotalRecords = firstUser?.Total ?? 0,
                Page = page,
                Limit = limit,
                Data = usersData
            };

            return ResponseObject<PagedResult<UserResponse>>.CreateResponse("Lấy dữ liệu thành công.", pagedResult);
        }



        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            using var multi = await connection.QueryMultipleAsync(
                "User_GetByEmail",
                new { Email = email }, 
                commandType: CommandType.StoredProcedure);

            // Đọc thông tin người dùng
            var user = await multi.ReadSingleOrDefaultAsync<UserDto>()
                        ?? throw new CustomException(StatusCodes.Status404NotFound, "Người dùng không tồn tại.");

            // Đọc danh sách vai trò
            var roles = await multi.ReadAsync<RoleDto>();
            user.Roles = roles.ToList(); // Gán danh sách vai trò cho user

            return user;
        }
    }

}

