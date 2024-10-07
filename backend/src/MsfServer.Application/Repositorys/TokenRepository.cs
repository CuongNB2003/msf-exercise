using Dapper;
using Microsoft.AspNetCore.Http;
using MsfServer.Application.Contracts.Token;
using MsfServer.Application.Contracts.Token.Dto;
using MsfServer.Application.Dapper;
using MsfServer.Domain.Shared.Exceptions;
using MsfServer.Domain.Shared.Responses;
using System.Data;

namespace MsfServer.Application.Repositorys
{
    public class TokenRepository(string connectionString) : ITokenRepository
    {
        private readonly string _connectionString = connectionString;
        public async Task<TokenDto> GetTokenAsync(string refreshToken)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();

            var token = await connection.QuerySingleOrDefaultAsync<TokenDto>(
                "Token_GetByRefreshToken",
                new { RefreshToken = refreshToken },
                commandType: CommandType.StoredProcedure);

            return token ?? throw new CustomException(StatusCodes.Status404NotFound, "Không tìm thấy Token.");
        }

        public async Task<ResponseText> SaveTokenAsync(TokenDto input)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();

            var parameters = new
            {
                input.UserId,
                input.RefreshToken,
                input.ExpirationDate
            };

            await connection.ExecuteAsync("Token_Save", parameters, commandType: CommandType.StoredProcedure);

            return ResponseText.ResponseSuccess("Cập nhật token thành công.", StatusCodes.Status204NoContent);
        }


        public async Task<ResponseText> DeleteTokenAsync(string idUser)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var result = await connection.ExecuteAsync(
                "Token_DeleteByUserId",
                new { UserIdString = idUser },
                commandType: CommandType.StoredProcedure);

            return ResponseText.ResponseSuccess("Xóa token thành công.", StatusCodes.Status204NoContent);
        }


    }
}
