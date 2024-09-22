using Microsoft.AspNetCore.Mvc;
using MsfServer.Application.Contracts.Users;
using MsfServer.Application.Contracts.Users.UserDtos;

namespace MsfServer.HttpApi
{
    [Route("api/user")]
    [ApiController]
    public class UsersController(IUserRepository userRepository) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;

        [HttpGet("{page}, {limit}")] // lấy tất cả users
        public async Task<IActionResult> GetUsers(int limit, int page)
        {
            var users = await _userRepository.GetUsersAsync(page, limit);
            return Ok(users);
        }

        [HttpGet("{id}")] // lấy user theo id
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpPost] // thêm user
        public async Task<IActionResult> CreateUser(UserInput user)
        {
            var result = await _userRepository.CreateUserAsync(user);
            return Ok(result);
        }

        [HttpPut("{id}")] // sửa user
        public async Task<IActionResult> UpdateUser(int id, UserInput input)
        {
            var result = await _userRepository.UpdateUserAsync(input, id);
            return Ok(result);
        }

        [HttpDelete("{id}")] // xóa user
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userRepository.DeleteUserAsync(id);
            return Ok(result);
        }

    }
}

