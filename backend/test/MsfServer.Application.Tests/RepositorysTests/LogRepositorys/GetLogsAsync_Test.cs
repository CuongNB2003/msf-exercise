
using Dapper;
using Moq;
using MsfServer.Application.Contracts.Log.LogDtos;
using MsfServer.Application.Repositories;
using MsfServer.Domain.Shared.Exceptions;
using System.Data;

namespace MsfServer.Application.Tests.RepositorysTests.LogRepositorys
{
    public class GetLogsAsync_Test
    {
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly LogRepository _repository;

        public GetLogsAsync_Test()
        {
            _mockConnection = new Mock<IDbConnection>();
            _repository = new LogRepository(DataBase.connectionString);
        }

        [Fact] // truyền tham số vào
        public async Task GetLogsAsync_ShouldReturnPagedResult_WhenPageAndLimitAreValid()
        {
            int page = 1;
            int limit = 10;
            var result = await _repository.GetLogsAsync(page, limit);
            Assert.NotNull(result);
            Assert.Equal(page, result.Data!.Page);
            Assert.Equal(limit, result.Data.Limit);
            Assert.True(result.Data.TotalRecords > 0);
            Assert.NotEmpty(result.Data.Data);
        }

        [Fact] // không truyền tham số
        public async Task GetLogsAsync_ShouldThrowCustomException_WhenPageOrLimitIsInvalid()
        {
            await Assert.ThrowsAsync<CustomException>(() => _repository.GetLogsAsync(0, 10));
            await Assert.ThrowsAsync<CustomException>(() => _repository.GetLogsAsync(1, 0));
        }
    }
}
