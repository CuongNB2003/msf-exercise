
using Dapper;
using Microsoft.AspNetCore.Http;
using Moq;
using MsfServer.Application.Contracts.Token.TokenDtos;
using MsfServer.Application.Repositorys;
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

            //_mockConnection.Setup(conn => conn.ExecuteAsync(
            //    "SaveToken",
            //    It.IsAny<object>(),
            //    null,
            //    null,
            //    CommandType.StoredProcedure))
            //    .ReturnsAsync(1);

            // Inject mock connection vào repository
            //var repository = new LogRepository(_mockConnection.Object);

            // Act
            var result = await _repository.SaveTokenAsync(tokenDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Cập nhật token thành công.", result.Message);
            Assert.Equal(StatusCodes.Status204NoContent, result.Status);
        }
    }
}
