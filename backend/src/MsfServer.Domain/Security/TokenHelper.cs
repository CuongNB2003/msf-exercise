using System.IdentityModel.Tokens.Jwt;

namespace MsfServer.HttpApi.Helper
{
    public static class TokenHelper
    {
        public static int GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.First(claim => claim.Type == "sub").Value;
            return int.Parse(userIdClaim);
        }
    }
}
