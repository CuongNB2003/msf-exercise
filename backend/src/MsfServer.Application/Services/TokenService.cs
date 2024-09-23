using Microsoft.IdentityModel.Tokens;
using MsfServer.Application.Contracts.Authentication;
using MsfServer.Application.Contracts.Token;
using MsfServer.Application.Contracts.Token.TokenDtos;
using MsfServer.Application.Contracts.Users;
using MsfServer.Application.Contracts.Users.UserDtos;
using MsfServer.Domain.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MsfServer.Application.Services
{
    public class TokenService(ITokenRepository tokenRepository, JwtSettings jwtSettings) : ITokenService
    {
        private readonly ITokenRepository _tokenRepository = tokenRepository;
        private readonly JwtSettings _jwtSettings = jwtSettings;
        // tạo ra AccessToken
        public async Task<TokenResultDto> GenerateAccessToken(UserDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key!);

            // Lấy các claim từ hàm GetClaims
            var claims = await GetClaims(user);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes);

            return TokenResultDto.ResponseToken(tokenString, expiresAt);
        }
        // tạo ra RefreshToken
        public Task<TokenResultDto> GenerateRefreshToken(int id)
        {
            throw new NotImplementedException();
        }
        // lấy GetClaims
        public async Task<IEnumerable<Claim>> GetClaims(UserDto user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.Role, user.Role!.Name!) // Sử dụng thuộc tính Name của RoleResultDto
            };

            return await Task.FromResult(claims);
        }
        //lấy refreshToken từ db
        public Task<TokenDto> GetToken(string refreshToken)
        {
            throw new NotImplementedException();
        }
        // tạo lại AccessToken
        public Task<TokenResultDto> RefreshAccessToken(string refreshToken)
        {
            throw new NotImplementedException();
        }
        // lưu token vào db
        public Task SaveToken(TokenDto tokenDto)
        {
            throw new NotImplementedException();
        }
    }
}
