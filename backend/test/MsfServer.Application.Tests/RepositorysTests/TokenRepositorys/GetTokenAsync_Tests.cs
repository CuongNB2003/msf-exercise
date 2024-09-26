
using Dapper;
using Moq;
using MsfServer.Application.Contracts.Token.TokenDtos;
using MsfServer.Application.Repositorys;
using MsfServer.Domain.Shared.Exceptions;
using System.Data;

namespace MsfServer.Application.Tests.RepositorysTests.TokenRepositorys
{
    public class GetTokenAsync_Tests
    {
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly TokenRepository _repository;

        public GetTokenAsync_Tests()
        {
            _mockConnection = new Mock<IDbConnection>();
            _repository = new TokenRepository(DataBase.connectionString);
        }

        [Fact]
        public async Task GetTokenAsync_ShouldReturnToken_WhenRefreshTokenExists()
        {
            string refreshToken = "cd52e4e0-f7ae-4bb8-a752-c4d80a4687bb";
            var result = await _repository.GetTokenAsync(refreshToken);
            Assert.NotNull(result);
            Assert.Equal(refreshToken, result.RefreshToken);
        }

        [Fact]
        public async Task GetTokenAsync_ShouldThrowCustomException_WhenRefreshTokenDoesNotExist()
        {
            // Arrange
            string refreshToken = "non_existing_refresh_token";
            await Assert.ThrowsAsync<CustomException>(() => _repository.GetTokenAsync(refreshToken));
        }
    }
}
