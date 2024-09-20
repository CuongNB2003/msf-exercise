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
        public async Task<IActionResult> GetRoles(int page, int limit)
        {
            try
            {
                var roles = await _roleRepository.GetRolesAsync(page, limit);
                return RequestSuccess.OK(roles, "Lấy dữ liệu thành công");
            }
            catch (Exception ex)
            {
                return RequestError.InternalServerError(ex.Message, "Lỗi khi lấy dữ liệu");
            }
        }

        [HttpGet("{id}")] // lấy theo id
        public async Task<IActionResult> GetRole(int id)
        {
            try
            {
                var role = await _roleRepository.GetRoleByIdAsync(id);
                if (role == null)
                {
                    return RequestError.NotFound($"ID: {id}", "Không tìm thấy Role");
                }
                return RequestSuccess.OK(role, "Lấy theo id thành công");
            }
            catch (Exception ex)
            {
                return RequestError.InternalServerError(ex.Message, "Lỗi khi lấy dữ liệu theo ID");
            }
        }

        [HttpPost] // tạo role 
        public async Task<IActionResult> CreateRole(RoleInput role)
        {
            try
            {
                var result = await _roleRepository.CreateRoleAsync(role);
                if (result > 0)
                {
                    return RequestSuccess.Create(role, "Thêm thành công");
                }
                else if (result == -1)
                {
                    return RequestError.BadRequest(role, "Role đã tồn tại");
                }
                return RequestError.BadRequest(role, "Thêm thất bại");
            }
            catch (Exception ex)
            {
                return RequestError.InternalServerError(ex.Message, "Lỗi khi tạo role");
            }
        }

        [HttpPut("{id}")] // sửa role
        public async Task<IActionResult> UpdateRole(int id, RoleInput input)
        {
            try
            {
                var role = await _roleRepository.GetRoleByIdAsync(id);
                if (role == null)
                {
                    return RequestError.BadRequest($"ID: {id}", "ID không tồn tại");
                }

                var result = await _roleRepository.UpdateRoleAsync(input, id);
                if (result > 0)
                {
                    return RequestSuccess.NoContent(result, "Sửa thành công");
                }
                else if (result == -1)
                {
                    return RequestError.BadRequest($"ID: {id}", "Role đã tồn tại không thể sửa");
                }
                return RequestError.NotFound($"ID: {id}", "Sửa thất bại");
            }
            catch (Exception ex)
            {
                return RequestError.InternalServerError(ex.Message, "Lỗi khi sửa role");
            }
        }

        [HttpDelete("{id}")] // xóa role
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var role = await _roleRepository.GetRoleByIdAsync(id);
                if (role == null)
                {
                    return RequestError.BadRequest($"ID: {id}", "ID không tồn tại");
                }
                var result = await _roleRepository.DeleteRoleAsync(id);
                if (result > 0)
                {
                    return RequestSuccess.NoContent($"ID: {id}", "Xóa thành công");
                }
                return RequestError.NotFound($"ID: {id}", "Xóa thất bại");
            }
            catch (Exception ex)
            {
                return RequestError.InternalServerError(ex.Message, "Lỗi khi xóa role");
            }
        }
    }
}
