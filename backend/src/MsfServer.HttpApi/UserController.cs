using Microsoft.AspNetCore.Mvc;
using MsfServer.Domain.users;
using MsfServer.Application.Contracts.Users;
using MsfServer.Application.Contracts.Users.UserDto;
using MsfServer.Domain.roles;
using MsfServer.HttpApi.ConfigRequests;
using System.Data;

namespace MsfServer.HttpApi
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository) => _userRepository = userRepository;

        [HttpGet] // lấy tất cả users
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userRepository.GetUsersAsync();
                return RequestSuccess.OK(users, "Lấy dữ liệu thành công");
            }
            catch (Exception ex)
            {
                return RequestError.InternalServerError("Đã xảy ra lỗi khi lấy dữ liệu", ex.Message);
            }
        }

        [HttpGet("{id}")] // lấy user theo id
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    return RequestError.NotFound($"ID: {id}", "User không tồn tại");
                }
                return RequestSuccess.OK(user, "Lấy theo id thành công");
            }
            catch (Exception ex)
            {
                return RequestError.InternalServerError("Đã xảy ra lỗi khi lấy dữ liệu", ex.Message);
            }
        }

        [HttpPost] // thêm user
        public async Task<IActionResult> CreateUser(UserInput user)
        {
            try
            {
                await _userRepository.CreateUserAsync(user);
                return RequestSuccess.Create(user, "Thêm thành công");
            }
            catch (Exception ex)
            {
                return RequestError.InternalServerError("Đã xảy ra lỗi khi thêm user", ex.Message);
            }
        }

        [HttpPut("{id}")] // sửa user
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            try
            {
                if (id != user.Id)
                {
                    return RequestError.BadRequest($"ID: {id}", "User không tồn tại");
                }
                var result = await _userRepository.UpdateUserAsync(user);
                if (result > 0)
                {
                    return RequestSuccess.NoContent(user, "Sửa thành công");
                }
                return RequestError.NotFound($"ID: {id}", "Sửa thất bại");
            }
            catch (Exception ex)
            {
                return RequestError.InternalServerError("Đã xảy ra lỗi khi sửa user", ex.Message);
            }
        }

        [HttpDelete("{id}")] // xóa user
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userRepository.DeleteUserAsync(id);
                if (result > 0)
                {
                    return RequestSuccess.NoContent($"ID: {id}", "Xóa user thành công");
                }
                return RequestError.NotFound($"ID: {id}", "Xóa thất bại");
            }
            catch (Exception ex)
            {
                return RequestError.InternalServerError("Đã xảy ra lỗi khi xóa user", ex.Message);
            }
        }

    }
}

