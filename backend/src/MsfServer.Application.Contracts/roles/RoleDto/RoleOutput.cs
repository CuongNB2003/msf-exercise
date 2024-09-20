using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsfServer.Application.Contracts.Roles.RoleDto
{
    public class RoleOutput
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
