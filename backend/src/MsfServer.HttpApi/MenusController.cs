
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MsfServer.Application.Contracts.Menu;
using MsfServer.Application.Contracts.Menu.Dto;
using MsfServer.HttpApi.Sercurity;

namespace MsfServer.HttpApi
{
    [Route("api/menu")]
    [ApiController]
    [Authorize(Policy = "PermissionPolicy")]
    public class MenusController(IMenuRepository menuRepository) : ControllerBase
    {
        private readonly IMenuRepository _menuRepository = menuRepository;

        [HttpGet] // lấy tất cả dữ liệu
        [AuthorizePermission(AuthorPermission.Menu.View)]
        public async Task<IActionResult> GetMenus(int page, int limit)
        {
            var roles = await _menuRepository.GetMenusAsync(page, limit);
            return Ok(roles);
        }

        [HttpGet("{id}")]
        //[AuthorizePermission(AuthorPermission.Menu.View)]
        public async Task<IActionResult> GetMenu(int id)
        {
            var role = await _menuRepository.GetMenuByIdAsync(id);
            return Ok(role);
        }

        [HttpPost]
        [AuthorizePermission(AuthorPermission.Menu.Create)]
        public async Task<IActionResult> CreateMenu(MenuCreateInput role)
        {
            var result = await _menuRepository.CreateMenuAsync(role);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [AuthorizePermission(AuthorPermission.Menu.Update)]
        public async Task<IActionResult> UpdateMenu(int id, MenuUpdateInput input)
        {
            var result = await _menuRepository.UpdateMenuAsync(input, id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [AuthorizePermission(AuthorPermission.Menu.Delete)]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            var result = await _menuRepository.DeleteMenuAsync(id);
            return Ok(result);
        }
    }
}
