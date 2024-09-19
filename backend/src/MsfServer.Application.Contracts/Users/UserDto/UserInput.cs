using MsfServer.Domain.roles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsfServer.Application.Contracts.Users.UserDto
{
    public class UserInput
    {
        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Password { get; set; }

        [Required]
        public int RoleId { get; set; }

        [MaxLength(255)]
        public string? Avatar { get; set; }

        [ForeignKey("RoleId")]
        public required Role Role { get; set; }
    }
}
