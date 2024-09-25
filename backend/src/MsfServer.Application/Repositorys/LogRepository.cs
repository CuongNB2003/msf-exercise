
using Dapper;
using Microsoft.AspNetCore.Http;
using MsfServer.Application.Contracts.Log;
using MsfServer.Application.Contracts.Log.LogDtos;
using MsfServer.Application.Database;
using MsfServer.Application.Page;
using MsfServer.Domain.Shared.Exceptions;
using MsfServer.Domain.Shared.Responses;
using System.Data;

namespace MsfServer.Application.Repositorys
{
    public class LogRepository(string connectionString, ResponseObject<LogDto> response) : ILogRepository
    {
        private readonly string _connectionString = connectionString;
        private readonly ResponseObject<LogDto> _response = response;

        public async Task<ResponseObject<PagedResult<LogDto>>> GetLogsAsync(int page, int limit)
        {
            if (page <= 0 || limit <= 0)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Bạn cần phải truyền vào page và limit.");
            }

            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();

            var offset = (page - 1) * limit;

            using var multi = await connection.QueryMultipleAsync(
                "GetPagedUserLogs",
                new { Offset = offset, PageSize = limit },
                commandType: CommandType.StoredProcedure);

            var totalRecords = await multi.ReadSingleAsync<int>();
            var userLogs = await multi.ReadAsync<dynamic>();

            var userLogDtos = userLogs.Select(ul => new LogDto
            {
                UserId = ul.UserId,
                Path = ul.Path,
                Method = ul.Method,
                RoleName = ul.UserRole,
                UserEmail = ul.UserEmail,
                UserName = ul.UserName,
            }).ToList();

            var pagedResult = new PagedResult<LogDto>
            {
                TotalRecords = totalRecords,
                Page = page,
                Limit = limit,
                Data = userLogDtos
            };

            return new ResponseObject<PagedResult<LogDto>>(StatusCodes.Status200OK, "Lấy dữ liệu thành công.", pagedResult);
        }





    }
}
