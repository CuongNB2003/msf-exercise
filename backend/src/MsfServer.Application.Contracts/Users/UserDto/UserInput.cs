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
        public string Email { get; set; } = string.Empty; // Ensure Name is not null

        [Required(ErrorMessage = "RoleId là bắt buộc.")]
        public int RoleId { get; set; }

        [MaxLength(255)]
        public string? Avatar { get; set; }
    }
}
