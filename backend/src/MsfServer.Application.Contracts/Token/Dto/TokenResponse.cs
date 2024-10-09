
namespace MsfServer.Application.Contracts.Token.Dto
{
    public class TokenResponse
    {
        public string?  Token { get; set; }
        public DateTime Expires { get; set; }

        public TokenResponse() { }

        public TokenResponse(string token, DateTime expires)
        {
            Token = token;
            Expires = expires;
        }

        public static TokenResponse ResponseToken(string token, DateTime expires)
        {
            return new TokenResponse(token, expires);
        }
    }
}
