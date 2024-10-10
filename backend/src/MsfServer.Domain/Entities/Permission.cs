
using System.ComponentModel.DataAnnotations;

namespace MsfServer.Domain.Entities
{
    public class Permission : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string? PermissionName { get; set; }  // Tên của Permission (ví dụ: Create, Edit, Delete)
        [MaxLength(255)]
        public string? Description { get; set; }  // Mô tả về quyền cụ thể

        // Điều hướng
        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
