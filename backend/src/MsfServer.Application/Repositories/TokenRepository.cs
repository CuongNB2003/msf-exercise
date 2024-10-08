using Dapper;
using Microsoft.AspNetCore.Http;
using MsfServer.Application.Contracts.Token;
using MsfServer.Application.Contracts.Token.Dto;
using MsfServer.Application.Dapper;
using MsfServer.Domain.Shared.Exceptions;
using MsfServer.Domain.Shared.Responses;
using Newtonsoft.Json;
using System.Data;

namespace MsfServer.Application.Repositories
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

            // Tạo JSON từ đối tượng TokenDto
            var jsonInput = new TokenJsonDto
            {
                UserId = input.UserId,
                RefreshToken = input.RefreshToken,
                ExpirationDate = input.ExpirationDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            };

            // Chuyển đổi đối tượng thành chuỗi JSON
            string json = JsonConvert.SerializeObject(jsonInput);

            var parameters = new { JsonInput = json };

            // Thực hiện gọi Stored Procedure với một tham số JSON
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
