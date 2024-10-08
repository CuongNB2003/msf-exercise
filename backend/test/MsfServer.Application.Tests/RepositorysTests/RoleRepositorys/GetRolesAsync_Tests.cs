using Microsoft.AspNetCore.Http;
using Moq;
using MsfServer.Application.Contracts.Role.RoleDtos;
using MsfServer.Application.Repositories;
using MsfServer.Domain.Shared.Exceptions;
using System.Data;

namespace MsfServer.Application.Tests.RepositorysTests.RoleRepositorys
{
    public class GetRolesAsync_Tests
    {
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly RoleRepository _repository;

        public GetRolesAsync_Tests()
        {
            _mockConnection = new Mock<IDbConnection>();
            _repository = new RoleRepository(DataBase.connectionString);
        }

        
        [Fact] // truyền tham số vào
        public async Task GetRolesAsync_ShouldReturnPagedResult_WhenPageAndLimitAreValid()
        {
            // Arrange
            int page = 1;
            int limit = 10;

            // Act
            var result = await _repository.GetRolesAsync(page, limit);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(page, result.Data!.Page);
            Assert.Equal(limit, result.Data.Limit);
            Assert.True(result.Data.TotalRecords > 0);
            Assert.NotEmpty(result.Data.Data);
        }

        [Fact] // không truyền tham số
        public async Task GetRolesAsync_ShouldThrowCustomException_WhenPageOrLimitIsInvalid()
        {
            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => _repository.GetRolesAsync(0, 10));
            await Assert.ThrowsAsync<CustomException>(() => _repository.GetRolesAsync(1, 0));
        }
    }
}

