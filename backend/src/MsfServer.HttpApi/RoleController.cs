using Microsoft.AspNetCore.Mvc;
using MsfServer.Application.Contracts.roles;
using MsfServer.Application.Contracts.Roles.RoleDtos;

namespace MsfServer.HttpApi
{
    [Route("api/role")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RolesController(IRoleRepository roleRepository) => _roleRepository = roleRepository;

        [HttpGet("{page}, {limit}")] // lấy tất cả dữ liệu
        public async Task<IActionResult> GetRoles(int page, int limit)
        {
            var roles = await _roleRepository.GetRolesAsync(page, limit);
            return Ok(roles);
        }

        [HttpGet("{id}")] // lấy theo id
        public async Task<IActionResult> GetRole(int id)
        {
            var role = await _roleRepository.GetRoleByIdAsync(id);
            return Ok(role);
        }

        [HttpPost] // tạo role 
        public async Task<IActionResult> CreateRole(RoleInputDto role)
        {
            var result = await _roleRepository.CreateRoleAsync(role);
            return Ok(result);
        }

        [HttpPut("{id}")] // sửa role
        public async Task<IActionResult> UpdateRole(int id, RoleInputDto input)
        {
            var result = await _roleRepository.UpdateRoleAsync(input, id);
            return Ok(result);
        }

        [HttpDelete("{id}")] // xóa role
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _roleRepository.DeleteRoleAsync(id);
            return Ok(result);
        }
    }
}
