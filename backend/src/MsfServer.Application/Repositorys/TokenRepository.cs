using Dapper;
using Microsoft.AspNetCore.Http;
using MsfServer.Application.Contracts.Token;
using MsfServer.Application.Contracts.Token.TokenDtos;
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

            // Query to get the token by refreshToken
            var token = await connection.QuerySingleOrDefaultAsync<TokenDto>(
                "SELECT * FROM Tokens WHERE RefreshToken = @RefreshToken", new { RefreshToken = refreshToken });

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

            await connection.ExecuteAsync("SaveToken", parameters, commandType: CommandType.StoredProcedure);

            return ResponseText.ResponseSuccess("Cập nhật token thành công.", StatusCodes.Status204NoContent);
        }


        public async Task<ResponseText> DeleteTokenAsync(string idUser)
        {
            if (!int.TryParse(idUser, out int userId))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Invalid user ID.");
            }

            // Kiểm tra xem token có tồn tại không
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();

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
