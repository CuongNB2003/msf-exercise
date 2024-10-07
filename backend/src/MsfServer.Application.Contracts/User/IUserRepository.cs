using MsfServer.Application.Contracts.User.Dto;
using MsfServer.Domain.Shared.PagedResults;
using MsfServer.Domain.Shared.Responses;

namespace MsfServer.Application.Contracts.User
{
    public interface IUserRepository
    {
        // hàm trong controller
        Task<ResponseObject<PagedResult<UserResponse>>> GetUsersAsync(int page, int limit);
        Task<ResponseObject<UserResponse>> GetUserByIdAsync(int id);
        Task<ResponseText> CreateUserAsync(CreateUserInput user);
        Task<ResponseText> UpdateUserAsync(UpdateUserInput user, int id);
        Task<ResponseText> DeleteUserAsync(int id);
        // hàm phụ việc truy vấn
        Task<bool> CheckEmailExistsAsync(string email);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> GetUserAsync(int id);

    }
}
