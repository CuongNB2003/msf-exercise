using MsfServer.Application.Contracts.User.Dto;
using System.ComponentModel.DataAnnotations.Schema;

namespace MsfServer.Application.Contracts.Token.Dto
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
