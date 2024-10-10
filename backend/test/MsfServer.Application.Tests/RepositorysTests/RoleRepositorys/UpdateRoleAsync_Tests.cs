using Microsoft.AspNetCore.Http;
using Moq;
using MsfServer.Application.Repositories;
using MsfServer.Domain.Shared.Exceptions;
using System.Data;

namespace MsfServer.Application.Tests.RepositorysTests.RoleRepositorys
{
    public class UpdateRoleAsync_Tests
    {
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly RoleRepository _repository;

        public UpdateRoleAsync_Tests()
        {
            _mockConnection = new Mock<IDbConnection>();
            _repository = new RoleRepository(DataBase.connectionString);
        }

        [Fact] // sửa name với id đúng
        public async Task UpdateRoleAsync_ShouldReturnSuccess_WhenRoleIsUpdated()
        {
            var input = new Contracts.Role.Dto.RoleInput { Name = "updated_role" };
            int roleId = 6;
            var result = await _repository.UpdateRoleAsync(input, roleId);
            Assert.Equal("Sửa thành công.", result.Message);
            Assert.Equal(StatusCodes.Status204NoContent, result.Status);
        }
        [Fact] // sửa role với id đúng nhưng name đã tồn tại
        public async Task UpdateRoleAsync_ShouldThrowCustomException_WhenRoleNameExists()
        {
            var input = new Contracts.Role.Dto.RoleInput { Name = "admin" };
            int roleId = 7;
            await Assert.ThrowsAsync<CustomException>(() => _repository.UpdateRoleAsync(input, roleId));
        }
    }
}
