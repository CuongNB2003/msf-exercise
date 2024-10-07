
using MsfServer.Application.Contracts.Token.Dto;

namespace MsfServer.Application.Contracts.Authentication.AuthDto
{
    public class TokenLogin
    {
        public TokenResponse? AccessToken { get; set; }
        public TokenResponse? RefreshToken { get; set; }

        public static TokenLogin GetToken(TokenResponse accessToken, TokenResponse refreshToken)
        {
            return new TokenLogin
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
