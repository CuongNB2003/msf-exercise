using Microsoft.AspNetCore.Http;
using Moq;
using MsfServer.Application.Contracts.Role.Dto;
using MsfServer.Application.Repositories;
using MsfServer.Domain.Shared.Exceptions;
using System.Data;

namespace MsfServer.Application.Tests.RepositorysTests.RoleRepositorys
{
    public class CreateRoleAsync_Tests
    {
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly RoleRepository _repository;

        public CreateRoleAsync_Tests()
        {
            _mockConnection = new Mock<IDbConnection>();
            _repository = new RoleRepository(DataBase.connectionString);
        }
        
        // hàm này dùng để tạo role mới
        [Fact] // tạo role mới 
        public async Task CreateRoleAsync_ShouldReturnSuccess_WhenRoleIsCreated()
        {
            var input = new RoleInput { Name = "ấdfasdf" };
            var result = await _repository.CreateRoleAsync(input);
            Assert.Equal("Thêm thành công.", result.Message);
            Assert.Equal(StatusCodes.Status201Created, result.Status);
        }
        [Fact] // tạo role mới nhưng name đã tồn tại
        public async Task CreateRoleAsync_ShouldThrowCustomException_WhenRoleNameExists()
        {
            var input = new RoleInput { Name = "user" };
            await Assert.ThrowsAsync<CustomException>(() => _repository.CreateRoleAsync(input));
        }
    }
}
