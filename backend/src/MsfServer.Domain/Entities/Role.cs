using System.ComponentModel.DataAnnotations;

namespace MsfServer.Domain.Entities
{
    public class Role : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }  // Tên của Role (ví dụ: Admin, User)
        [MaxLength(255)]
        public string? Description { get; set; }  // Mô tả về vai trò

        // Điều hướng
        public ICollection<RolePermission>? RolePermissions { get; set; }
        public ICollection<UserRole>? UserRoles { get; set; }
    }
}
