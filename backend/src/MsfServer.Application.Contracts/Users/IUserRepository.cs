using MsfServer.Application.Contracts.Users.UserDto;
using MsfServer.Application.Page;
using MsfServer.Domain.Shared.Responses;

namespace MsfServer.Application.Contracts.Users
{
    public interface IUserRepository
    {
        Task<ResponseObject<PagedResult<UserOutput>>> GetUsersAsync(int page, int limit);
        Task<ResponseObject<UserOutput>> GetUserByIdAsync(int id);
        Task<ResponseText> CreateUserAsync(UserInput user);
        Task<ResponseText> UpdateUserAsync(UserInput user, int id);
        Task<ResponseText> DeleteUserAsync(int id);
        Task<bool> CheckEmailExistsAsync(string email);
        Task<bool> CheckUserExistsAsync(int id);
        Task<string> GetUserEmailByIdAsync(int id);

    }
}
