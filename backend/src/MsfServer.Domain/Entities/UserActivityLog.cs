
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MsfServer.Domain.Entities
{
    public class UserActivityLog : BaseModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Path { get; set; }

        [Required]
        [MaxLength(10)]
        public string? Method { get; set; }

        // Thêm quan hệ với bảng Users
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}
