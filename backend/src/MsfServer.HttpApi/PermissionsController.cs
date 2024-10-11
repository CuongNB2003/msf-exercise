
using Microsoft.AspNetCore.Mvc;
using MsfServer.Application.Contracts.Permission;
using MsfServer.Application.Contracts.Permission.Dto;
using MsfServer.Application.Contracts.Role;
using MsfServer.Application.Contracts.Role.Dto;

namespace MsfServer.HttpApi
{
    [Route("api/permission")]
    [ApiController]
    //[Authorize(Roles = "admin,user")]
    public class PermissionsController(IPermissionRepository permissionRepository) : ControllerBase
    {
        private readonly IPermissionRepository _permissionRepository = permissionRepository;

        [HttpGet]
        public async Task<IActionResult> GetRoles(int page, int limit)
        {
            var roles = await _permissionRepository.GetPermissionsAsync(page, limit);
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(int id)
        {
            var role = await _permissionRepository.GetPermissionByIdAsync(id);
            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(PermissionInput role)
        {
            var result = await _permissionRepository.CreatePermissionAsync(role);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, PermissionInput input)
        {
            var result = await _permissionRepository.UpdatePermissionAsync(input, id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _permissionRepository.DeletePermissionAsync(id);
            return Ok(result);
        }
    }
}
