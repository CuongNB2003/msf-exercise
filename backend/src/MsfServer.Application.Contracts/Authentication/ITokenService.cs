using MsfServer.Application.Contracts.Authentication.AuthDto;
using MsfServer.Application.Contracts.Token.Dto;
using MsfServer.Application.Contracts.User.Dto;
using MsfServer.Domain.Shared.Responses;
using System.Security.Claims;

namespace MsfServer.Application.Contracts.Authentication
{
    public interface ITokenService
    {
        Task<TokenResponse> GenerateAccessTokenAsync(UserDto user); // tạo AccessToken
        Task<TokenResponse> GenerateRefreshTokenAsync(UserDto user); // tạo RefreshToken
        Task<IEnumerable<Claim>> GetClaimsAsync(UserDto user);
        Task<ResponseObject<TokenLogin>> RefreshAccessTokenAsync(string refreshToken); // làm mới AccessToken
    }
}
