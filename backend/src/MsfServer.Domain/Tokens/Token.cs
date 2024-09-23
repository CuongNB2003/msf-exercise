using MsfServer.Domain.users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsfServer.Domain.Tokens
{
    public class Token
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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
