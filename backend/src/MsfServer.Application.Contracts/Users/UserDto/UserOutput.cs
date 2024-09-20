using MsfServer.Application.Contracts.Roles.RoleDto;
using System.ComponentModel.DataAnnotations.Schema;

namespace MsfServer.Application.Contracts.Users.UserDto
{
    public class UserOutput
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int RoleId { get; set; }
        public string? Avatar { get; set; }

        [ForeignKey("RoleId")]
        public required RoleOutput Role { get; set; }
    }
}
