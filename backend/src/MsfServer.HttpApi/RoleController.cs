using Microsoft.AspNetCore.Mvc;
using MsfServer.Application.Contracts.roles;
using MsfServer.Application.Contracts.Roles.RoleDto;
using MsfServer.Domain.roles;
using MsfServer.HttpApi.ConfigRequests;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MsfServer.HttpApi
{
    [Route("api/role")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RolesController(IRoleRepository roleRepository) => _roleRepository = roleRepository;


        [HttpGet] // lấy tất cả dữ liệu
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleRepository.GetRolesAsync();
            return RequestSuccess.OK(roles, "Lấy dữ liệu thành công");
        }

        [HttpGet("{id}")] // lấy theo id
        public async Task<IActionResult> GetRole(int id)
        {
            var role = await _roleRepository.GetRoleByIdAsync(id);
            if (role == null)
            {
                return RequestError.NotFound($"ID: {id}", "Role không tồn tại");
            }
            return RequestSuccess.OK(role, "Lấy theo id thành công");
        }

        [HttpPost] // tạo role 
        public async Task<IActionResult> CreateRole(RoleInput role)
        {
            var result = await _roleRepository.CreateRoleAsync(role);
            if (result > 0)
            {
                return RequestSuccess.Create(role, "Thêm thành công");
            }else if(result == -1)
            {
                return RequestError.BadRequest(role, "Role đã tồn tại");
            }
            return RequestError.BadRequest(role, "Thêm thất bại");
        }

        [HttpPut("{id}")] // sửa role
        public async Task<IActionResult> UpdateRole(int id, Role role)
        {
            if (id != role.Id)
            {
                return RequestError.BadRequest($"ID: {id}", "ID không tổn tại");
            }

            var result = await _roleRepository.UpdateRoleAsync(role);
            if (result > 0)
            {
                return RequestSuccess.NoContent(role, "Sửa thành công");
            }
            return RequestError.NotFound($"ID: {id}", "Sửa thất bại");
        }

        [HttpDelete("{id}")] // xóa role
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _roleRepository.DeleteRoleAsync(id);
            if (result > 0)
            {
                return RequestSuccess.NoContent($"ID: {id}", "Xóa thành công");
            }
            return RequestError.NotFound($"ID: {id}", "Xóa thất bại");
        }

    }
}
