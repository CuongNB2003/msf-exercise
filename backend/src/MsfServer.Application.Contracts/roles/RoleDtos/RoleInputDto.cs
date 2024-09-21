using System.ComponentModel.DataAnnotations;

namespace MsfServer.Application.Contracts.Roles.RoleDtos
{
    public class RoleInputDto
    {
        [Required(ErrorMessage = "Tên quyền là bắt buộc.")]
        public string Name { get; set; } = string.Empty;
    }
}
