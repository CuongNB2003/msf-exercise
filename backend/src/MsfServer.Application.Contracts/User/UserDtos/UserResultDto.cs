using MsfServer.Application.Contracts.Role.RoleDtos;
using System.ComponentModel.DataAnnotations.Schema;

namespace MsfServer.Application.Contracts.User.UserDtos
{
    public class UserResultDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int RoleId { get; set; }
        public string? Avatar { get; set; }

        [ForeignKey("RoleId")]
        public required RoleResultDto Role { get; set; }
    }
}
