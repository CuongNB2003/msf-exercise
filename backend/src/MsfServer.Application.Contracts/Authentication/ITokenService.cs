using MsfServer.Application.Contracts.Token.TokenDtos;
using MsfServer.Application.Contracts.Users.UserDtos;
using System.Security.Claims;

namespace MsfServer.Application.Contracts.Authentication
{
    public interface ITokenService
    {
        Task<TokenResultDto> GenerateAccessTokenAsync(UserDto user); // tạo AccessToken
        Task<TokenResultDto> GenerateRefreshTokenAsync(UserDto user); // tạo RefreshToken
        Task<IEnumerable<Claim>> GetClaimsAsync(UserDto user);
        Task<TokenResultDto> RefreshAccessTokenAsync(string refreshToken); // làm mới AccessToken

    }
}
