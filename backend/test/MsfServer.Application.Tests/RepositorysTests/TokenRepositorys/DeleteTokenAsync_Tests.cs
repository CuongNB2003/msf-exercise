using Dapper;
using Microsoft.AspNetCore.Http;
using Moq;
using MsfServer.Application.Contracts.Token.TokenDtos;
using MsfServer.Application.Repositorys;
using MsfServer.Domain.Entities;
using MsfServer.Domain.Shared.Exceptions;
using System.Data;
using System.Threading.Tasks;
using Xunit;

namespace MsfServer.Application.Tests.RepositorysTests.TokenRepositorys
{
    public class DeleteTokenAsync_Tests
    {
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly TokenRepository _repository;

        public DeleteTokenAsync_Tests()
        {
            _mockConnection = new Mock<IDbConnection>();
            _repository = new TokenRepository(DataBase.connectionString);
        }

        [Fact]
        public async Task DeleteTokenAsync_ShouldThrowCustomException_WhenUserIdIsInvalid()
        {
            // Arrange
            string invalidUserId = "abc";

            await Assert.ThrowsAsync<CustomException>(() => _repository.DeleteTokenAsync(invalidUserId));
        }

        [Fact]
        public async Task DeleteTokenAsync_ShouldThrowCustomException_WhenTokenNotFound()
        {
            string validUserId = "100";
            await Assert.ThrowsAsync<CustomException>(() => _repository.DeleteTokenAsync(validUserId));
        }

        [Fact]
        public async Task DeleteTokenAsync_ShouldReturnSuccessResponse_WhenTokenDeleted()
        {
            string validUserId = "1";
            var result = await _repository.DeleteTokenAsync(validUserId);
            Assert.Equal("Xóa token thành công.", result.Message);
            Assert.Equal(StatusCodes.Status204NoContent, result.Status);
        }
    }
}
