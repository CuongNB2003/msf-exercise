using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MsfServer.Application.Contracts.Role;
using MsfServer.Application.Contracts.Role.Dto;

namespace MsfServer.HttpApi
{
    [Route("api/role")]
    [ApiController]
    //[Authorize(Roles = "admin,user")]
    public class RolesController(IRoleRepository roleRepository) : ControllerBase
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        [HttpGet] // lấy tất cả dữ liệu
        public async Task<IActionResult> GetRoles(int page, int limit)
        {
            //Thang thuoc phong ban nào moi thục hiện tiếp theo
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
        public async Task<IActionResult> CreateRole(RoleInput role)
        {
            var result = await _roleRepository.CreateRoleAsync(role);
            return Ok(result);
        }

        [HttpPut("{id}")] // sửa role
        public async Task<IActionResult> UpdateRole(int id, RoleInput input)
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
