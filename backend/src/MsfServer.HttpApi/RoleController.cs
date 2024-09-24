using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MsfServer.Application.Contracts.Role;
using MsfServer.Application.Contracts.Role.RoleDtos;

namespace MsfServer.HttpApi
{
    [Route("api/role")]
    [ApiController]
    public class RolesController(IRoleRepository roleRepository) : ControllerBase
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        [Authorize(Roles = "admin,user")]
        [HttpGet] // lấy tất cả dữ liệu
        public async Task<IActionResult> GetRoles(int page, int limit)
        {
            var roles = await _roleRepository.GetRolesAsync(page, limit);
            return Ok(roles);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")] // lấy theo id
        public async Task<IActionResult> GetRole(int id)
        {
            var role = await _roleRepository.GetRoleByIdAsync(id);
            return Ok(role);
        }

        [Authorize(Roles = "admin")]
        [HttpPost] // tạo role 
        public async Task<IActionResult> CreateRole(RoleInputDto role)
        {
            var result = await _roleRepository.CreateRoleAsync(role);
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")] // sửa role
        public async Task<IActionResult> UpdateRole(int id, RoleInputDto input)
        {
            var result = await _roleRepository.UpdateRoleAsync(input, id);
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")] // xóa role
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _roleRepository.DeleteRoleAsync(id);
            return Ok(result);
        }
    }

}
