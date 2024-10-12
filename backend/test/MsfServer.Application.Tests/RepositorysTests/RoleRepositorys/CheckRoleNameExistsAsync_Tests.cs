using Moq;
using MsfServer.Application.Repositories;
using System.Data;

namespace MsfServer.Application.Tests.RepositorysTests.RoleRepositorys
{
    public class CheckRoleNameExistsAsync_Tests
    {
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly RoleRepository _repository;

        public CheckRoleNameExistsAsync_Tests()
        {
            _mockConnection = new Mock<IDbConnection>();
            _repository = new RoleRepository(DataBase.connectionString);
        }

        // hàm này sẽ kiểm tra xem role nam có tồn tại hay không
        //[Fact] // điều kiện đúng
        //public async Task CheckRoleNameExistsAsync_ShouldReturnTrue_WhenRoleNameExists()
        //{
        //    string roleName = "admin";
        //    var result = await _repository.CheckRoleNameExistsAsync(roleName);
        //    Assert.True(result);
        //}

        //[Fact] // điều kiện sai 
        //public async Task CheckRoleNameExistsAsync_ShouldReturnFalse_WhenRoleNameDoesNotExist()
        //{
        //    string roleName = "non_existing_role";
        //    var result = await _repository.CheckRoleNameExistsAsync(roleName);
        //    Assert.False(result);
        //}


    }
}
