using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsfServer.Application.Contracts.Roles.RoleDto
{
    public class RoleInput
    {
        [Required(ErrorMessage = "Tên quyền là bắt buộc.")]
        public string? Name { get; set; }
    }
}
