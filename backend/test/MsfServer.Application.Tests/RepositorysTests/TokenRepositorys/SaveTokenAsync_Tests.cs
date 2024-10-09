
using Dapper;
using Microsoft.AspNetCore.Http;
using Moq;
using MsfServer.Application.Contracts.Token.TokenDtos;
using MsfServer.Application.Repositories;
using System.Data;

namespace MsfServer.Application.Tests.RepositorysTests.TokenRepositorys
{
    public class SaveTokenAsync_Tests
    {
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly TokenRepository _repository;

        public SaveTokenAsync_Tests()
        {
            _mockConnection = new Mock<IDbConnection>();
            _repository = new TokenRepository(DataBase.connectionString);
        }
        [Fact]
        public async Task SaveTokenAsync_ShouldReturnSuccess_WhenTokenIsSaved()
        {
            // Arrange
            var tokenDto = new TokenDto
            {
                UserId = 1,
                RefreshToken = "sample_refresh_token",
                ExpirationDate = DateTime.UtcNow.AddDays(7)
            };
            // Act
            var result = await _repository.SaveTokenAsync(tokenDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Cập nhật token thành công.", result.Message);
            Assert.Equal(StatusCodes.Status204NoContent, result.Status);
        }
    }
}
