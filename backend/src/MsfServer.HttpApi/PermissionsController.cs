
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MsfServer.Application.Contracts.Permission;
using MsfServer.Application.Contracts.Permission.Dto;
using MsfServer.Application.Contracts.Role;
using MsfServer.Application.Contracts.Role.Dto;
using MsfServer.HttpApi.Sercurity;

namespace MsfServer.HttpApi
{
    [Route("api/permission")]
    [ApiController]
    [Authorize(Policy = "PermissionPolicy")]
    public class PermissionsController(IPermissionRepository permissionRepository) : ControllerBase
    {
        private readonly IPermissionRepository _permissionRepository = permissionRepository;

        [HttpGet]
        [AuthorizePermission(AuthorPermission.Permission.View)]
        public async Task<IActionResult> GetPermissions(int page, int limit)
        {
            var roles = await _permissionRepository.GetPermissionsAsync(page, limit);
            return Ok(roles);
        }

        [HttpGet("{id}")]
        //[AuthorizePermission(AuthorPermission.Permission.View)]
        public async Task<IActionResult> GetPermission(int id)
        {
            var role = await _permissionRepository.GetPermissionByIdAsync(id);
            return Ok(role);
        }

        [HttpPost]
        [AuthorizePermission(AuthorPermission.Permission.Create)]
        public async Task<IActionResult> CreatePermission(PermissionInput role)
        {
            var result = await _permissionRepository.CreatePermissionAsync(role);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [AuthorizePermission(AuthorPermission.Permission.Update)]
        public async Task<IActionResult> UpdatePermission(int id, PermissionInput input)
        {
            var result = await _permissionRepository.UpdatePermissionAsync(input, id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [AuthorizePermission(AuthorPermission.Permission.Delete)]
        public async Task<IActionResult> DeletePermission(int id)
        {
            var result = await _permissionRepository.DeletePermissionAsync(id);
            return Ok(result);
        }
    }
}
