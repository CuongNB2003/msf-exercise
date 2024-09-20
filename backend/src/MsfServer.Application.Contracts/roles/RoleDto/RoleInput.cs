using System.ComponentModel.DataAnnotations;

namespace MsfServer.Application.Contracts.Roles.RoleDto
{
    public class RoleInput
    {
        [Required(ErrorMessage = "Tên quyền là bắt buộc.")]
        public string? Name { get; set; }
    }
}
