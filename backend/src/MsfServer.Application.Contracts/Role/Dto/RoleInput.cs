using System.ComponentModel.DataAnnotations;

namespace MsfServer.Application.Contracts.Role.Dto
{
    public class RoleInput
    {
        [Required(ErrorMessage = "Tên quyền là bắt buộc.")]
        public string Name { get; set; } = string.Empty;
        [MaxLength(250, ErrorMessage = "Không được nhập quá 250 kí tự.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Phải có ít nhất một MenuId.")]
        public List<int> MenuIds { get; set; } = [];

        [Required(ErrorMessage = "Phải có ít nhất một PermissionId.")]
        public List<int> PermissionIds { get; set; } = [];
    }
}
