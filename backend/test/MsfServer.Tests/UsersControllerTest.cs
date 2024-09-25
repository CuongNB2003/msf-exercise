
using Microsoft.AspNetCore.Mvc;
using Moq;
using MsfServer.Application.Contracts.Role.RoleDtos;
using MsfServer.Application.Contracts.User;
using MsfServer.Application.Contracts.User.UserDtos;
using MsfServer.Domain.Shared.PagedResults;
using MsfServer.Domain.Shared.Responses;

namespace MsfServer.HttpApi.Tests
{
    public class UsersControllerTest
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly UsersController _controller;

        public UsersControllerTest()
        {
            _mockRepo = new Mock<IUserRepository>();
            _controller = new UsersController(_mockRepo.Object);
        }

        [Fact] // test controller lấy tất cả user
        public async Task GetUsers_ReturnsOkResult_WithListOfUsers()
        {
            // tạo dữ liệu mẫu
            var users = new PagedResult<UserResultDto>
            {
                Data = [new UserResultDto { Id = 1, Name = "CuongNB", Email = "cuong@gmail.com", RoleId = 1 }],
                TotalRecords = 1,
                Limit = 1,
                Page = 1
            };
            var responseObject = new ResponseObject<PagedResult<UserResultDto>> { Data = users, Message = "cuong dang test", Status = 200 };
            _mockRepo.Setup(repo => repo.GetUsersAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(responseObject);

            // kiểm thử
            var result = await _controller.GetUsers(1, 10);

            // kết quả
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ResponseObject<PagedResult<UserResultDto>>>(okResult.Value);
            Assert.Single(returnValue.Data!.Data);
        }

        [Fact] // test controller lấy user theo id
        public async Task GetUser_ReturnsOkResult_WithUser()
        {
            // tạo dữ liệu mẫu
            var user = new UserResultDto { Id = 1, Name = "Admin" };
            var responseObject = new ResponseObject<UserResultDto> { Data = user };
            _mockRepo.Setup(repo => repo.GetUserByIdAsync(1)).ReturnsAsync(responseObject);

            // kiểm thử
            var result = await _controller.GetUser(1);

            // kết quả
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ResponseObject<UserResultDto>>(okResult.Value);
            Assert.Equal(1, returnValue.Data!.Id);
        }

        [Fact] // test controller tạo user
        public async Task CreateUser_ReturnsOkResult_WithCreatedUser()
        {
            // tạo dữ liệu mẫu
            var userInput = new CreateUserInput { Email = "cuong@gmail.com", RoleId = 1, Avatar = "" };
            var responseText = new ResponseText { Message = "User created successfully", Status = 201 };
            _mockRepo.Setup(repo => repo.CreateUserAsync(userInput)).ReturnsAsync(responseText);

            // kiểm thử
            var result = await _controller.CreateUser(userInput);

            // kết quả
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ResponseText>(okResult.Value);
            Assert.Equal("User created successfully", returnValue.Message);
        }

        [Fact] // test controller update user
        public async Task UpdateUser_ReturnsOkResult_WithUpdatedUser()
        {
            // tạo dữ liệu mẫu
            var roleInput = new UpdateUserInput { Name = "lại là cường đây", Avatar = "", RoleId  = 1, Email = "cuong@gmail.com"};
            var responseText = new ResponseText { Message = "User updated successfully", Status = 204};
            _mockRepo.Setup(repo => repo.UpdateUserAsync(roleInput, 1)).ReturnsAsync(responseText);

            // kiểm thử
            var result = await _controller.UpdateUser(1, roleInput);

            // kết quả
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ResponseText>(okResult.Value);
            Assert.Equal("User updated successfully", returnValue.Message);
        }

        [Fact] // test controller xóa user
        public async Task DeleteUser_ReturnsOkResult()
        {
            // tạo dữ liệu mẫu
            var responseText = new ResponseText { Message = "User deleted successfully", Status = 204 };
            _mockRepo.Setup(repo => repo.DeleteUserAsync(1)).ReturnsAsync(responseText);

            // kiểm thử
            var result = await _controller.DeleteUser(1);

            // kết quả
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ResponseText>(okResult.Value);
            Assert.Equal("User deleted successfully", returnValue.Message);
        }

    }
}
