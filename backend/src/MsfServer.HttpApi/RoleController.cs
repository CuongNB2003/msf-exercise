using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MsfServer.HttpApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RolesController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            var roles = await _roleRepository.GetRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            var role = await _roleRepository.GetRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        [HttpPost]
        public async Task<ActionResult> CreateRole(Role role)
        {
            var result = await _roleRepository.CreateRoleAsync(role);
            if (result > 0)
            {
                return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRole(int id, Role role)
        {
            if (id != role.Id)
            {
                return BadRequest();
            }

            var result = await _roleRepository.UpdateRoleAsync(role);
            if (result > 0)
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRole(int id)
        {
            var result = await _roleRepository.DeleteRoleAsync(id);
            if (result > 0)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
