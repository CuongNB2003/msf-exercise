using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MsfServer.Application.Contracts.Role;
using MsfServer.Application.Contracts.Role.Dto;
using MsfServer.HttpApi.Sercurity;

namespace MsfServer.HttpApi
{
    [Route("api/role")]
    [ApiController]
    [Authorize(Policy = "PermissionPolicy")]
    public class RolesController(IRoleRepository roleRepository) : ControllerBase
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        [HttpGet]
        [AuthorizePermission(AuthorPermission.Role.View)]
        public async Task<IActionResult> GetRoles(int page, int limit)
        {
            var roles = await _roleRepository.GetRolesAsync(page, limit);
            return Ok(roles);
        }

        [HttpGet("{id}")]
        //[AuthorizePermission(AuthorPermission.Role.View)]
        public async Task<IActionResult> GetRole(int id)
        {
            var role = await _roleRepository.GetRoleByIdAsync(id);
            return Ok(role);
        }

        [HttpPost]
        [AuthorizePermission(AuthorPermission.Role.Create)]
        public async Task<IActionResult> CreateRole(RoleInput role)
        {
            var result = await _roleRepository.CreateRoleAsync(role);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [AuthorizePermission(AuthorPermission.Role.Update)]
        public async Task<IActionResult> UpdateRole(int id, RoleInput input)
        {
            var result = await _roleRepository.UpdateRoleAsync(input, id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [AuthorizePermission(AuthorPermission.Role.Delete)]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _roleRepository.DeleteRoleAsync(id);
            return Ok(result);
        }
    }

}
