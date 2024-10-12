
using System.ComponentModel.DataAnnotations;

namespace MsfServer.Domain.Entities
{
    public class MenuEntity  : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string? DisplayName { get; set; }
        [Required, Url]
        [MaxLength(100)]
        public string? Url { get; set; }
        [MaxLength(50)]
        public string? IconName { get; set; }
        public bool Status { get; set; } = true;
    }
}
