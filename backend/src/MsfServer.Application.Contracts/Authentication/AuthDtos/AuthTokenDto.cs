
using MsfServer.Application.Contracts.Token.TokenDtos;
using MsfServer.Application.Contracts.Users.UserDtos;

namespace MsfServer.Application.Contracts.Authentication.AuthDtos
{
    public class AuthTokenDto
    {
        public TokenResultDto? AccessToken { get; set; }
        public TokenResultDto? RefreshToken { get; set; }

        public static AuthTokenDto GetToken(TokenResultDto accessToken, TokenResultDto refreshToken)
        {
            return new AuthTokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
