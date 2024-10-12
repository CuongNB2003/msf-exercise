
using Moq;
using MsfServer.Application.Repositories;
using MsfServer.Domain.Shared.Exceptions;
using System.Data;

namespace MsfServer.Application.Tests.RepositorysTests.LogRepositorys
{
    public class GetLogByIdAsync_Tests
    {
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly LogRepository _repository;

        public GetLogByIdAsync_Tests()
        {
            _mockConnection = new Mock<IDbConnection>();
            _repository = new LogRepository(DataBase.connectionString);
        }


        [Fact] // id đúng
        public async Task GetRoleByIdAsync_ShouldReturnRole_WhenRoleIdExists()
        {
            int roleId = 165;
            var result = await _repository.GetLogByIdAsync(roleId);
            Assert.NotNull(result);
            Assert.Equal(roleId, result.Data!.Id);
        }
        [Fact] // id sai 
        public async Task GetRoleByIdAsync_ShouldThrowCustomException_WhenRoleIdDoesNotExist()
        {
            int roleId = -1;
            await Assert.ThrowsAsync<CustomException>(() => _repository.GetLogByIdAsync(roleId));
        }

    }
}
