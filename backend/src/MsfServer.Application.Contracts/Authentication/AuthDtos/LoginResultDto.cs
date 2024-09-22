
using MsfServer.Application.Contracts.Users.UserDtos;

namespace MsfServer.Application.Contracts.Authentication.AuthDtos
{
    public class LoginResultDto
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
        public UserLoginDto? User { get; set; }
    }
}
