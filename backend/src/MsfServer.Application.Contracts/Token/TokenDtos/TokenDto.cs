using MsfServer.Application.Contracts.Users.UserDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsfServer.Application.Contracts.Token.TokenDtos
{
    public class TokenDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpirationDate { get; set; }
        [ForeignKey("UserId")]
        public UserDto? User { get; set; }
    }
}
