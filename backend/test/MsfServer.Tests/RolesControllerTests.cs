using Microsoft.AspNetCore.Mvc;
using Moq;
using MsfServer.Application.Contracts.Role;
using MsfServer.Application.Contracts.Role.RoleDtos;
using MsfServer.Domain.Shared.PagedResults;
using MsfServer.Domain.Shared.Responses;

namespace MsfServer.HttpApi.Tests
{
    public class RolesControllerTests
    {
        private readonly Mock<IRoleRepository> _mockRepo;
        private readonly RolesController _controller;

        public RolesControllerTests()
        {
            _mockRepo = new Mock<IRoleRepository>();
            _controller = new RolesController(_mockRepo.Object);
        }

        [Fact] // test controller lấy tất cả role
        public async Task GetRoles_ReturnsOkResult_WithListOfRoles()
        {
            // Arrange
            var roles = new PagedResult<RoleResultDto>
            {
                Data = [new RoleResultDto { Id = 1, Name = "Admin" }],
                TotalRecords = 1,
                Limit = 1,
                Page = 1
            };
            var responseObject = new ResponseObject<PagedResult<RoleResultDto>> { Data = roles };
            _mockRepo.Setup(repo => repo.GetRolesAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(responseObject);

            // Act
            var result = await _controller.GetRoles(1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ResponseObject<PagedResult<RoleResultDto>>>(okResult.Value);
            Assert.Single(returnValue.Data!.Data);
        }

        [Fact] // test controller lấy role theo id
        public async Task GetRole_ReturnsOkResult_WithRole()
        {
            // Arrange
            var role = new RoleResultDto { Id = 1, Name = "Admin" };
            var responseObject = new ResponseObject<RoleResultDto> { Data = role };
            _mockRepo.Setup(repo => repo.GetRoleByIdAsync(1)).ReturnsAsync(responseObject);

            // Act
            var result = await _controller.GetRole(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ResponseObject<RoleResultDto>>(okResult.Value);
            Assert.Equal(1, returnValue.Data!.Id);
        }

        [Fact] // test controller tạo role
        public async Task CreateRole_ReturnsOkResult_WithCreatedRole()
        {
            // Arrange
            var roleInput = new RoleInputDto { Name = "User" };
            var responseText = new ResponseText { Message = "Role created successfully" };
            _mockRepo.Setup(repo => repo.CreateRoleAsync(roleInput)).ReturnsAsync(responseText);

            // Act
            var result = await _controller.CreateRole(roleInput);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ResponseText>(okResult.Value);
            Assert.Equal("Role created successfully", returnValue.Message);
        }

        [Fact] // test controller update role
        public async Task UpdateRole_ReturnsOkResult_WithUpdatedRole()
        {
            // Arrange
            var roleInput = new RoleInputDto { Name = "SuperAdmin" };
            var responseText = new ResponseText { Message = "Role updated successfully" };
            _mockRepo.Setup(repo => repo.UpdateRoleAsync(roleInput, 1)).ReturnsAsync(responseText);

            // Act
            var result = await _controller.UpdateRole(1, roleInput);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ResponseText>(okResult.Value);
            Assert.Equal("Role updated successfully", returnValue.Message);
        }

        [Fact] // test controller xóa role
        public async Task DeleteRole_ReturnsOkResult()
        {
            // Arrange
            var responseText = new ResponseText { Message = "Role deleted successfully" };
            _mockRepo.Setup(repo => repo.DeleteRoleAsync(1)).ReturnsAsync(responseText);

            // Act
            var result = await _controller.DeleteRole(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ResponseText>(okResult.Value);
            Assert.Equal("Role deleted successfully", returnValue.Message);
        }
    }
}