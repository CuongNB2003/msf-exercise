using Microsoft.IdentityModel.Tokens;
using MsfServer.Application.Contracts.Authentication;
using MsfServer.Application.Contracts.Authentication.AuthDto;
using MsfServer.Application.Contracts.Token;
using MsfServer.Application.Contracts.Token.Dto;
using MsfServer.Application.Contracts.User.Dto;
using MsfServer.Application.Contracts.User;
using MsfServer.Domain.Security;
using MsfServer.Domain.Shared.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MsfServer.Application.Services
{
    public class TokenService(
        ITokenRepository tokenRepository,
        IUserRepository userRepository,
        JwtSettings jwtSettings
        ) : ITokenService
    {
        private readonly ITokenRepository _tokenRepository = tokenRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly JwtSettings _jwtSettings = jwtSettings;
        // tạo ra AccessToken
        public async Task<TokenResponse> GenerateAccessTokenAsync(UserResponse user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key!);

            // Lấy các claim từ hàm GetClaims
            var claims = await GetClaimsAsync(user);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes),
                Audience = _jwtSettings.Audience,
                Issuer = _jwtSettings.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes);

            return TokenResponse.ResponseToken(tokenString, expiresAt);
        }
        // tạo ra RefreshToken
        public async Task<TokenResponse> GenerateRefreshTokenAsync(UserResponse user)
        {
            // Generate a new refresh token
            var refreshToken = new TokenResponse
            {
                Token = Guid.NewGuid().ToString(),
                Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
                // Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes)
            };

            var tokenDto = new TokenDto
            {
                UserId = user.Id,
                RefreshToken = refreshToken.Token,
                ExpirationDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
                // ExpirationDate = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes)
            };

            // Save the refresh token to the repository
            await _tokenRepository.SaveTokenAsync(tokenDto);

            return refreshToken;
        }
        // lấy GetClaims
        public async Task<IEnumerable<Claim>> GetClaimsAsync(UserResponse user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Name, user.Name!),
                new(JwtRegisteredClaimNames.Email, user.Email!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (user.Roles != null)
            {
                foreach (var role in user.Roles)
                {
                    // Đảm bảo role là chuỗi (string)
                    claims.Add(new Claim(ClaimTypes.Role, role.Id.ToString()));
                }
            }

            return await Task.FromResult(claims);
        }

        // tạo lại AccessToken
        public async Task<ResponseObject<TokenLogin>> RefreshAccessTokenAsync(string refreshToken)
        {
            var token = await _tokenRepository.GetTokenAsync(refreshToken);
            var user = await _userRepository.GetUserByIdAsync(token.UserId);
            var accessTokenNew = await GenerateAccessTokenAsync(user.Data!);
            var refreshTokenNew = await GenerateRefreshTokenAsync(user.Data!);
            return ResponseObject<TokenLogin>.CreateResponse("Khởi tạo token thành công.", TokenLogin.GetToken(accessTokenNew, refreshTokenNew));
        }
    }
}
