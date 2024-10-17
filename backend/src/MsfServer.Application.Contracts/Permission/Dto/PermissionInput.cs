
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MsfServer.Application.Contracts.Permission.Dto
{
    public class PermissionInput
    {
        [
            Required(ErrorMessage = "Bạn cần phải nhập PermissionName."),
            MaxLength(50, ErrorMessage = "Không được nhập quá 50 kí tự."),
            RegularExpression(@"^[a-zA-Z0-9]+\.[a-zA-Z0-9]+$", ErrorMessage = "Tên phải phân tách bằng dấu chấm (Group.Action)")
        ]
        [DefaultValue("Group.Action")]
        public string? PermissionName { get; set; }  // Tên của Permission (ví dụ: Create, Edit, Delete)

        [Required(ErrorMessage = "Bạn cần phải nhập Name.")]
        [MaxLength(50, ErrorMessage = "Không được nhập quá 50 kí tự.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255, ErrorMessage = "Không được nhập quá 250 kí tự")]
        public string? Description { get; set; }  // Mô tả về quyền cụ thể
    }
}
