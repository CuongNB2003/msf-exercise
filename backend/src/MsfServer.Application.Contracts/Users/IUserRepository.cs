using Microsoft.AspNetCore.Mvc;
using MsfServer.Application.Contracts.Users.UserDto;
using MsfServer.Domain.roles;
using MsfServer.Domain.users;

namespace MsfServer.Application.Contracts.Users
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<int> CreateUserAsync(UserInput user);
        Task<int> UpdateUserAsync(User user);
        Task<int> DeleteUserAsync(int id);
    }
}
