
using Dapper;
using Microsoft.AspNetCore.Http;
using MsfServer.Application.Contracts.Log;
using MsfServer.Application.Contracts.Log.LogDtos;
using MsfServer.Application.Dapper;
using MsfServer.Domain.Shared.Exceptions;
using MsfServer.Domain.Shared.PagedResults;
using MsfServer.Domain.Shared.Responses;
using System.Data;

namespace MsfServer.Application.Repositorys
{
    public class LogRepository(string connectionString) : ILogRepository
    {
        private readonly string _connectionString = connectionString;
        public Task<ResponseObject<LogDto>> GetLogByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseObject<PagedResult<LogDto>>> GetLogsAsync(int page, int limit)
        {
            if (page <= 0 || limit <= 0)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Bạn cần phải truyền vào page và limit.");
            }
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();

            var offset = (page - 1) * limit;

            using var multi = await connection.QueryMultipleAsync(
                "GetPagedLogs",
                new { Offset = offset, PageSize = limit },
                commandType: CommandType.StoredProcedure);

            var totalRecords = await multi.ReadSingleAsync<int>();
            var userLogs = await multi.ReadAsync<LogDto>();

            var pagedResult = new PagedResult<LogDto>
            {
                TotalRecords = totalRecords,
                Page = page,
                Limit = limit,
                Data = userLogs.ToList()
            };

            return ResponseObject<PagedResult<LogDto>>.CreateResponse("Lấy dữ liệu thành công.", pagedResult);
        }

    }
}
