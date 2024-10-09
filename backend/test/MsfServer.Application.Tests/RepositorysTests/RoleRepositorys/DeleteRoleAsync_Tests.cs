using Microsoft.AspNetCore.Http;
using Moq;
using MsfServer.Application.Repositories;
using MsfServer.Domain.Shared.Exceptions;
using System.Data;

namespace MsfServer.Application.Tests.RepositorysTests.RoleRepositorys
{
    public class DeleteRoleAsync_Tests
    {
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly RoleRepository _repository;

        public DeleteRoleAsync_Tests()
        {
            _mockConnection = new Mock<IDbConnection>();
            _repository = new RoleRepository(DataBase.connectionString);
        }

        // Test hàm DeleteRoleAsync
        [Fact] // xóa với id đúng
        public async Task DeleteRoleAsync_ShouldReturnSuccess_WhenRoleIsDeleted()
        {
            int roleId = 5;
            var result = await _repository.DeleteRoleAsync(roleId);
            Assert.Equal("Xóa thành công.", result.Message);
            Assert.Equal(StatusCodes.Status204NoContent, result.Status);
        }
        [Fact] // xóa với id sai 
        public async Task DeleteRoleAsync_ShouldThrowCustomException_WhenRoleDoesNotExist()
        {
            int roleId = -1;
            await Assert.ThrowsAsync<CustomException>(() => _repository.DeleteRoleAsync(roleId));
        }
        [Fact] // xóa id đúng nhưng có user sử dụng role 
        public async Task DeleteRoleAsync_ShouldThrowCustomException_WhenRoleHasRelatedUsers()
        {
            int roleId = 3;
            await Assert.ThrowsAsync<CustomException>(() => _repository.DeleteRoleAsync(roleId));
        }
    }
}
