using Moq;
using MsfServer.Application.Repositories;
using MsfServer.Domain.Shared.Exceptions;
using System.Data;

namespace MsfServer.Application.Tests.RepositorysTests.RoleRepositorys
{
    public class GetRoleByIdAsync_Tests
    {
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly RoleRepository _repository;

        public GetRoleByIdAsync_Tests()
        {
            _mockConnection = new Mock<IDbConnection>();
            _repository = new RoleRepository(DataBase.connectionString);
        }

        [Fact] // id đúng
        public async Task GetRoleByIdAsync_ShouldReturnRole_WhenRoleIdExists()
        {
            int roleId = 5;
            var result = await _repository.GetRoleByIdAsync(roleId);
            Assert.NotNull(result);
            Assert.Equal(roleId, result.Data!.Id);
        }
        [Fact] // id sai 
        public async Task GetRoleByIdAsync_ShouldThrowCustomException_WhenRoleIdDoesNotExist()
        {
            int roleId = -1;
            await Assert.ThrowsAsync<CustomException>(() => _repository.GetRoleByIdAsync(roleId));
        }
    }
}
