using MsfServer.Application.Contracts.Token.TokenDtos;
using MsfServer.Domain.Shared.Responses;

namespace MsfServer.Application.Contracts.Token
{
    public interface ITokenRepository
    {
        Task<TokenDto> GetTokenAsync(string refreshToken);
        Task<ResponseText> SaveTokenAsync(TokenDto input);
        Task<ResponseText> DeleteTokenAsync(string idUser);
    }
}
