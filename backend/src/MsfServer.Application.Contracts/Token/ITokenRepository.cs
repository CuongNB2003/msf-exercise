﻿
using MsfServer.Application.Contracts.Roles.RoleDtos;
using MsfServer.Application.Contracts.Token.TokenDtos;
using MsfServer.Application.Page;
using MsfServer.Domain.Shared.Responses;

namespace MsfServer.Application.Contracts.Token
{
    public interface ITokenRepository
    {
        Task<TokenDto> GetTokenAsync(string refreshToken);
        Task<ResponseText> SaveTokenAsync(TokenDto input);
        Task<bool> CheckTokenUserIdExistsAsync(int idUser);
        Task<ResponseText> DeleteTokenAsync(string idUser);
    }
}
