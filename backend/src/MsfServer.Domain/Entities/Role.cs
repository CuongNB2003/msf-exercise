using System.ComponentModel.DataAnnotations;

namespace MsfServer.Domain.Entities
{
    public class Role : BaseModel
    {

        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }
    }
}
