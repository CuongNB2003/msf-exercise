
using MsfServer.Application.Contracts.Token.TokenDtos;
using MsfServer.Application.Contracts.Users.UserDtos;

namespace MsfServer.Application.Contracts.Authentication.AuthDtos
{
    public class LoginResultDto
    {
        public TokenResultDto? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? Expiration { get; set; }
        public UserLoginDto? User { get; set; }


        public static LoginResultDto CreateResult(TokenResultDto accessToken, string refreshToken, UserLoginDto user)
        {
            return new LoginResultDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expiration = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"),
                User = user
            };
        }
    }
}
