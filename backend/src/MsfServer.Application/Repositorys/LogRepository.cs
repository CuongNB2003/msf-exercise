﻿
using Dapper;
using Microsoft.AspNetCore.Http;
using MsfServer.Application.Contracts.Log;
using MsfServer.Application.Contracts.Log.Dto;
using MsfServer.Application.Contracts.Role.Dto;
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

        public async Task<ResponseObject<LogDto>> GetLogByIdAsync(int id)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var log = await connection.QuerySingleOrDefaultAsync<LogDto>(
            "Log_GetById", new { Id = id }, commandType: CommandType.StoredProcedure);

            return ResponseObject<LogDto>.CreateResponse("Lấy dữ liệu thành công.", log!);
        }

        public async Task<ResponseObject<PagedResult<LogDto>>> GetLogsAsync(int page, int limit)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            // Gọi stored procedure và truyền page, limit
            using var multi = await connection.QueryMultipleAsync(
                "Log_GetAll",
                new { Page = page, Limit = limit },
                commandType: CommandType.StoredProcedure);

            var userLogs = await multi.ReadAsync<LogDto>();
            var firstLog = userLogs.FirstOrDefault();
            // Tạo đối tượng PagedResult để trả về
            var pagedResult = new PagedResult<LogDto>
            {
                TotalRecords = firstLog?.TotalLog ?? 0,
                Page = page,
                Limit = limit,
                Data = userLogs.ToList() ?? new List<LogDto>()
            };

            return ResponseObject<PagedResult<LogDto>>.CreateResponse("Lấy dữ liệu thành công.", pagedResult);
        }
    }
}
