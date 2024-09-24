
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
                "SELECT * FROM Tokens WHERE RefreshToken = @RefreshToken", new { RefreshToken = refreshToken });

            return token ?? throw new CustomException(StatusCodes.Status404NotFound, "Không tìm thấy Token.");
        }

        public async Task<bool> CheckTokenUserIdExistsAsync(int idUser)
        {
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();
            var sql = "SELECT COUNT(1) FROM Tokens WHERE UserId = @UserId";
            var count = await connection.ExecuteScalarAsync<int>(sql, new { UserId = idUser });
            return count > 0;
        }

        public async Task<ResponseText> SaveTokenAsync(TokenDto input)
        {
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();

            if (!await CheckTokenUserIdExistsAsync(input.UserId))
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
                    SET RefreshToken = @RefreshToken, ExpirationDate = @ExpirationDate
                    WHERE UserId = @UserId";

                await connection.ExecuteAsync(updateQuery, new { input.UserId, input.RefreshToken, input.ExpirationDate });

            }
            return ResponseText.ResponseSuccess("Cập nhật token thành công.", StatusCodes.Status204NoContent);
        }

        public async Task<ResponseText> DeleteTokenAsync(string idUser)
        {
            if (!int.TryParse(idUser, out int userId))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Invalid user ID.");
            }

            // Kiểm tra xem token có tồn tại không
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();

            // Xóa các token liên quan đến userId
            var sql = "DELETE FROM Tokens WHERE UserId = @UserId";
            var result = await connection.ExecuteAsync(sql, new { UserId = userId });

            if (result > 0)
            {
                return ResponseText.ResponseSuccess("Xóa token thành công.", StatusCodes.Status204NoContent);
            }
            else
            {
                throw new CustomException(StatusCodes.Status404NotFound, "Không tìm thấy token cho người dùng này.");
            }
        }


    }
}
