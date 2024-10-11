
using Microsoft.AspNetCore.Mvc;
using MsfServer.Application.Contracts.Menu;
using MsfServer.Application.Contracts.Menu.Dto;
using MsfServer.Application.Contracts.Role;
using MsfServer.Application.Contracts.Role.Dto;

namespace MsfServer.HttpApi
{
    [Route("api/menu")]
    [ApiController]
    //[Authorize(Roles = "admin,user")]
    public class MenusController(IMenuRepository menuRepository) : ControllerBase
    {
        private readonly IMenuRepository _menuRepository = menuRepository;

        [HttpGet] // lấy tất cả dữ liệu
        public async Task<IActionResult> GetRoles(int page, int limit)
        {
            var roles = await _menuRepository.GetMenusAsync(page, limit);
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(int id)
        {
            var role = await _menuRepository.GetMenuByIdAsync(id);
            return Ok(role);
        }

        [HttpPost] 
        public async Task<IActionResult> CreateRole(MenuCreateInput role)
        {
            var result = await _menuRepository.CreateMenuAsync(role);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, MenuUpdateInput input)
        {
            var result = await _menuRepository.UpdateMenuAsync(input, id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _menuRepository.DeleteMenuAsync(id);
            return Ok(result);
        }
    }
}
