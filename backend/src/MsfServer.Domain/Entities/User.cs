using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MsfServer.Domain.Entities
{
    public class User : BaseEntity
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

        [MaxLength(255)]
        public string? Avatar { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Salt { get; set; }

        // Điều hướng
        public ICollection<UserRole>? UserRoles { get; set; }
    }
}