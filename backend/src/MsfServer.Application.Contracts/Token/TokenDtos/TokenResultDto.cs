
namespace MsfServer.Application.Contracts.Token.TokenDtos
{
    public class TokenResultDto
    {
        public string?  Token { get; set; }
        public DateTime Expires { get; set; }

        public TokenResultDto() { }

        public TokenResultDto(string token, DateTime expires)
        {
            Token = token;
            Expires = expires;
        }

        public static TokenResultDto ResponseToken(string token, DateTime expires)
        {
            return new TokenResultDto(token, expires);
        }
    }
}
