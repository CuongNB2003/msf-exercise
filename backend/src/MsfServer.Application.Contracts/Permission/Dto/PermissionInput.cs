
using System.ComponentModel.DataAnnotations;

namespace MsfServer.Application.Contracts.Permission.Dto
{
    public class PermissionInput
    {
        [Required(ErrorMessage = "Bạn cần phải nhập PermissionName.")]
        [MaxLength(50, ErrorMessage = "Không được nhập quá 50 kí tự.")]
        public string? PermissionName { get; set; }  // Tên của Permission (ví dụ: Create, Edit, Delete)
        [MaxLength(255, ErrorMessage = "Không được nhập quá 250 kí tự")]
        public string? Description { get; set; }  // Mô tả về quyền cụ thể
    }
}
