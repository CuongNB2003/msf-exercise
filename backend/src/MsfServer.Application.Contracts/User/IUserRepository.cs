﻿using MsfServer.Application.Contracts.User.UserDtos;
using MsfServer.Application.Contracts.Users.UserDtos;
using MsfServer.Application.Page;
using MsfServer.Domain.Shared.Responses;

namespace MsfServer.Application.Contracts.Users
{
    public interface IUserRepository
    {
        // hàm trong controller
        Task<ResponseObject<PagedResult<UserResultDto>>> GetUsersAsync(int page, int limit);
        Task<ResponseObject<UserResultDto>> GetUserByIdAsync(int id);
        Task<ResponseText> CreateUserAsync(UserInput user);
        Task<ResponseText> UpdateUserAsync(UserInput user, int id);
        Task<ResponseText> DeleteUserAsync(int id);
        // hàm phụ việc truy vấn
        Task<bool> CheckEmailExistsAsync(string email);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> GetUserAsync(int id);

    }
}