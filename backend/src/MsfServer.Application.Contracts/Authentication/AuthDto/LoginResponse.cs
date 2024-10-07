
namespace MsfServer.Application.Contracts.Authentication.AuthDto
{
    public class LoginResponse
    {
        public TokenLogin? Token { get; set; }
        public DateTime? Expiration { get; set; }
        public UserLogin? User { get; set; }


        public static LoginResponse CreateResult(TokenLogin token, UserLogin user)
        {
            return new LoginResponse
            {
                Token = token,
                Expiration = DateTime.Now,
                User = user
            };
        }
    }
}
