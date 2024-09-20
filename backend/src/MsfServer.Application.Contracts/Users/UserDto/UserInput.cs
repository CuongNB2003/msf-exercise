using MsfServer.Domain.roles;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MsfServer.Application.Contracts.Users.UserDto
{
    public class UserInput
    {
        [Required(ErrorMessage = "Name là bắt buộc.")]
        [MaxLength(50)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email chưa đúng định dạng.")]
        [MaxLength(50)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password là bắt buộc.")]
        [MaxLength(100)]
        [MinLength(6, ErrorMessage = "Password ít nhất 6 kí tự.")]

        public string? Password { get; set; }

        [Required(ErrorMessage = "RoleId là bắt buộc.")]
        public int RoleId { get; set; }

        [MaxLength(255)]
        public string? Avatar { get; set; }

        [ForeignKey("RoleId")]
        public required Role Role { get; set; }
    }
}
