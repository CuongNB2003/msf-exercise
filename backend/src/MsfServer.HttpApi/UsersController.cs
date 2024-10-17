using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MsfServer.Application.Contracts.User.Dto;
using MsfServer.Application.Contracts.User;
using MsfServer.HttpApi.Sercurity;

namespace MsfServer.HttpApi
{
    [Route("api/user")]
    [ApiController]
    [Authorize(Policy = "PermissionPolicy")]
    public class UsersController(IUserRepository userRepository) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;

        [HttpGet()]
        [AuthorizePermission(AuthorPermission.User.View)]
        public async Task<IActionResult> GetUsers(int page, int limit)
        {
            var users = await _userRepository.GetUsersAsync(page, limit);
            return Ok(users);
        }

        [HttpGet("{id}")] 
        //[AuthorizePermission(AuthorPermission.User.View)]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpPost]
        [AuthorizePermission(AuthorPermission.User.Create)]
        public async Task<IActionResult> CreateUser(CreateUserInput user)
        {
            var result = await _userRepository.CreateUserAsync(user);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [AuthorizePermission(AuthorPermission.User.Update)]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserInput input)
        {
            var result = await _userRepository.UpdateUserAsync(input, id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [AuthorizePermission(AuthorPermission.User.Delete)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userRepository.DeleteUserAsync(id);
            return Ok(result);
        }

    }
}

