using MsfServer.Application.Contracts.Roles.RoleDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsfServer.Application.Contracts.Authentication.AuthDtos
{
    public class UserLoginDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int RoleId { get; set; }
        public string? Avatar { get; set; }
        [ForeignKey("RoleId")]
        public required RoleResultDto Role { get; set; }
    }
}
