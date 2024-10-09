using System.ComponentModel.DataAnnotations;

namespace MsfServer.Application.Contracts.Role.Dto
{
    public class RoleInput
    {
        [Required(ErrorMessage = "Tên quyền là bắt buộc.")]
        public string Name { get; set; } = string.Empty;
    }
}
