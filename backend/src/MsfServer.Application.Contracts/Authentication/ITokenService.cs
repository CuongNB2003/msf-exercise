using MsfServer.Application.Contracts.Token.TokenDtos;
using MsfServer.Application.Contracts.Users.UserDtos;
using System.Security.Claims;

namespace MsfServer.Application.Contracts.Authentication
{
    public interface ITokenService
    {
        Task<TokenResultDto> GenerateAccessToken(UserDto user); // tạo AccessToken
        Task<TokenResultDto> GenerateRefreshToken(int id); // tạo RefreshToken
        Task<IEnumerable<Claim>> GetClaims(UserDto user);
        Task<TokenResultDto> RefreshAccessToken(string refreshToken); // làm mới AccessToken
        Task SaveToken(TokenDto tokenDto); // lưu RefreshToken vào db
        Task<TokenDto> GetToken(string refreshToken); // lấy RefreshToken trong db

    }
}
