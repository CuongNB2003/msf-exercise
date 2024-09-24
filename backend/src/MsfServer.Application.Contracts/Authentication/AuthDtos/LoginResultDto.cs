
namespace MsfServer.Application.Contracts.Authentication.AuthDtos
{
    public class LoginResultDto
    {
        public AuthTokenDto? Token { get; set; }
        public DateTime? Expiration { get; set; }
        public UserLoginDto? User { get; set; }


        public static LoginResultDto CreateResult(AuthTokenDto token, UserLoginDto user)
        {
            return new LoginResultDto
            {
                Token = token,
                Expiration = DateTime.Now,
                User = user
            };
        }
    }
}
