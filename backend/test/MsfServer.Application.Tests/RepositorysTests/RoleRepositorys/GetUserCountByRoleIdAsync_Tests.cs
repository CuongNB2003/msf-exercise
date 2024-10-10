using Moq;
using MsfServer.Application.Repositories;
using System.Data;

namespace MsfServer.Application.Tests.RepositorysTests.RoleRepositorys
{
    public class GetUserCountByRoleIdAsync_Tests
    {
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly RoleRepository _repository;

        public GetUserCountByRoleIdAsync_Tests()
        {
            _mockConnection = new Mock<IDbConnection>();
            _repository = new RoleRepository(DataBase.connectionString);
        }

        // kiểm tra xem role id có bao nhiêu user đang sử dụng
        //[Fact] // tham số đúng
        //public async Task GetUserCountByRoleIdAsync_ShouldReturnUserCount_WhenRoleIdExists()
        //{
        //    int roleId = 3;
        //    var result = await _repository.GetUserCountByRoleIdAsync(roleId);
        //    Assert.True(result >= 0);
        //}

        //[Fact] // tham số sai
        //public async Task GetUserCountByRoleIdAsync_ShouldReturnZero_WhenRoleIdDoesNotExist()
        //{
        //    int roleId = -1;
        //    var result = await _repository.GetUserCountByRoleIdAsync(roleId);
        //    Assert.Equal(0, result);
        //}

    }
}
