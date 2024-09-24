﻿using Microsoft.IdentityModel.Tokens;
using MsfServer.Application.Contracts.Authentication;
using MsfServer.Application.Contracts.Authentication.AuthDtos;
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
        public async Task<TokenResultDto> GenerateAccessTokenAsync(UserDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key!);

            // Lấy các claim từ hàm GetClaims
            var claims = await GetClaimsAsync(user);
            foreach (var claim in claims)
            {
                System.Diagnostics.Debug.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }

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

            return TokenResultDto.ResponseToken(tokenString, expiresAt);
        }
        // tạo ra RefreshToken
        public async Task<TokenResultDto> GenerateRefreshTokenAsync(UserDto user)
        {
            // Generate a new refresh token
            var refreshToken = new TokenResultDto
            {
                Token = Guid.NewGuid().ToString(),
                Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
            };

            var tokenDto = new TokenDto
            {
                UserId = user.Id,
                RefreshToken = refreshToken.Token,
                ExpirationDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
            };

            // Save the refresh token to the repository
            await _tokenRepository.SaveTokenAsync(tokenDto);

            return refreshToken;
        }
        // lấy GetClaims
        public async Task<IEnumerable<Claim>> GetClaimsAsync(UserDto user)
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
        // tạo lại AccessToken
        public  async Task<AuthTokenDto> RefreshAccessTokenAsync(string refreshToken)
        {
            var token  = await _tokenRepository.GetTokenAsync(refreshToken);
            var user = await _userRepository.GetUserAsync(token.UserId);
            var accessTokenNew = await GenerateAccessTokenAsync(user);
            var refreshTokenNew = await GenerateRefreshTokenAsync(user);
            return AuthTokenDto.GetToken(accessTokenNew, refreshTokenNew);
        }
        // xóa RefreshToken
        public async Task RevokeRefreshTokenAsync(string userId)
        {
            await _tokenRepository.DeleteTokenAsync(userId);
        }
    }
}
