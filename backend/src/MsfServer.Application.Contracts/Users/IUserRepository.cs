using MsfServer.Application.Contracts.Users.UserDto;
using MsfServer.Application.Page;
using MsfServer.Domain.users;

namespace MsfServer.Application.Contracts.Users
{
    public interface IUserRepository
    {
        Task<PagedResult<UserOutput>> GetUsersAsync(int page, int limit);
        Task<UserOutput> GetUserByIdAsync(int id);
        Task<int> CreateUserAsync(UserInput user);
        Task<int> UpdateUserAsync(UserInput user);
        Task<int> DeleteUserAsync(int id);
    }
}
