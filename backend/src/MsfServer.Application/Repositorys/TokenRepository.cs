
using Dapper;
using Microsoft.AspNetCore.Http;
using MsfServer.Application.Contracts.Roles.RoleDtos;
using MsfServer.Application.Contracts.Token;
using MsfServer.Application.Contracts.Token.TokenDtos;
using MsfServer.Application.Database;
using MsfServer.Domain.Shared.Exceptions;
using MsfServer.Domain.Shared.Responses;

namespace MsfServer.Application.Repositorys
{
    public class TokenRepository(string connectionString) : ITokenRepository
    {
        private readonly string _connectionString = connectionString;
        public async Task<TokenDto> GetTokenAsync(string refreshToken)
        {
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();

            // Query to get the token by refreshToken
            var token = await connection.QuerySingleOrDefaultAsync<TokenDto>(
                "SELECT * FROM Token WHERE RefreshToken = @RefreshToken", new { RefreshToken = refreshToken });

            return token ?? throw new CustomException(StatusCodes.Status404NotFound, "Không tìm thấy Token.");
        }

        public async Task<TokenDto> GetTokenByIdAsync(int id)
        {
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();

            // Query to get the token by Id
            var token = await connection.QuerySingleOrDefaultAsync<TokenDto>(
                "SELECT * FROM Token WHERE Id = @Id", new { Id = id });

            return token ?? throw new CustomException(StatusCodes.Status404NotFound, "Không tìm thấy Token.");
        }

        public async Task<ResponseText> SaveTokenAsync(TokenDto input)
        {
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();

            var token = await GetTokenByIdAsync(input.Id);

            if (token == null)
            {
                var insertQuery = @"
                    INSERT INTO Tokens (UserId, RefreshToken, ExpirationDate)
                    VALUES (@UserId, @RefreshToken, @ExpirationDate)";

                await connection.ExecuteAsync(insertQuery, new { input.UserId, input.RefreshToken, input.ExpirationDate });
            }
            else
            {
                var updateQuery = @"
                    UPDATE Tokens
                    SET UserId = @UserId, RefreshToken = @RefreshToken, ExpirationDate = @ExpirationDate
                    WHERE Id = @Id";

                await connection.ExecuteAsync(updateQuery, new { input.UserId, input.RefreshToken, input.ExpirationDate});
            }
            return ResponseText.ResponseSuccess("Cập nhật token thành công.", StatusCodes.Status204NoContent);
        }
    }
}
