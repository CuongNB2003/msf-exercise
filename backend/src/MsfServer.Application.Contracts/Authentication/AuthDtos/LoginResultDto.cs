
using MsfServer.Application.Contracts.Token.TokenDtos;
using MsfServer.Application.Contracts.Users.UserDtos;

namespace MsfServer.Application.Contracts.Authentication.AuthDtos
{
    public class LoginResultDto
    {
        public TokenResultDto? AccessToken { get; set; }
        public TokenResultDto? RefreshToken { get; set; }
        public string? Expiration { get; set; }
        public UserLoginDto? User { get; set; }


        public static LoginResultDto CreateResult(TokenResultDto accessToken, TokenResultDto refreshToken, UserLoginDto user)
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
