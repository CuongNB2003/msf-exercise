using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MsfServer.Domain.roles;

namespace MsfServer.Domain.users
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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
        public Role? Role { get; set; }
    }
}