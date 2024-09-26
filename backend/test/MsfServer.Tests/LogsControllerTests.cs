
using Microsoft.AspNetCore.Mvc;
using Moq;
using MsfServer.Application.Contracts.Role.RoleDtos;
using MsfServer.Application.Contracts.Role;
using MsfServer.Domain.Shared.PagedResults;
using MsfServer.Domain.Shared.Responses;
using MsfServer.Application.Contracts.Log;
using MsfServer.Application.Contracts.Log.LogDtos;

namespace MsfServer.HttpApi.Tests
{
    public class LogsControllerTests
    {
        private readonly Mock<ILogRepository> _mockRepo;
        private readonly LogsController _controller;

        public LogsControllerTests()
        {
            _mockRepo = new Mock<ILogRepository>();
            _controller = new LogsController(_mockRepo.Object);
        }

        [Fact] // test controller lấy tất cả logs
        public async Task GetLogs_ReturnsOkResult_WithListOfLogs()
        {
            // Arrange
            var logs = new PagedResult<LogDto>
            {
                Data = [new LogDto { 
                    Id = 1, 
                    UserName = "Admin", 
                    ClientIpAddress = "1.1.1", 
                    CreatedAt = DateTime.Now, 
                    Duration = 1, 
                    Method = "GET", 
                    StatusCode = 200, 
                    Url = "" 
                }],
                TotalRecords = 1,
                Limit = 1,
                Page = 1
            };
            var responseObject = new ResponseObject<PagedResult<LogDto>> { Data = logs };
            _mockRepo.Setup(repo => repo.GetLogsAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(responseObject);

            // Act
            var result = await _controller.GetLogs(1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ResponseObject<PagedResult<LogDto>>>(okResult.Value);
            Assert.Single(returnValue.Data!.Data);
        }

        [Fact] // test controller lấy log theo id
        public async Task GetLog_ReturnsOkResult_WithLog()
        {
            // Arrange
            var role = new LogDto
            {
                Id = 1,
                UserName = "Admin",
                ClientIpAddress = "1.1.1",
                CreatedAt = DateTime.Now,
                Duration = 1,
                Method = "GET",
                StatusCode = 200,
                Url = ""
            };
            var responseObject = new ResponseObject<LogDto> { Data = role };
            _mockRepo.Setup(repo => repo.GetLogByIdAsync(1)).ReturnsAsync(responseObject);

            // Act
            var result = await _controller.GetLog(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ResponseObject<LogDto>>(okResult.Value);
            Assert.Equal(1, returnValue.Data!.Id);
        }
    }
}
