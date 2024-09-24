using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MsfServer.Domain.Entities
{
    public class Token : BaseModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(256)]
        public string? RefreshToken { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
