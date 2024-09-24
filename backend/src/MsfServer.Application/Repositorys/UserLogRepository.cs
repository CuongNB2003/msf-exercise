
using Dapper;
using Microsoft.AspNetCore.Http;
using MsfServer.Application.Contracts.Role.RoleDtos;
using MsfServer.Application.Contracts.UserLog;
using MsfServer.Application.Contracts.UserLog.UserLogDtos;
using MsfServer.Application.Database;
using MsfServer.Application.Page;
using MsfServer.Domain.Entities;
using MsfServer.Domain.Shared.Exceptions;
using MsfServer.Domain.Shared.Responses;
using System.Data;

namespace MsfServer.Application.Repositorys
{
    public class UserLogRepository(string connectionString, ResponseObject<UserLogDto> response) : IUserLogRepository
    {
        private readonly string _connectionString = connectionString;
        private readonly ResponseObject<UserLogDto> _response = response;

        public async Task<ResponseText> CreateUserLogAsync(UserLogDto input)
        {
            using var dbManager = new DatabaseConnectionManager(_connectionString);
            using var connection = dbManager.GetOpenConnection();

            // Tạo đối tượng UserLogDto từ input
            var userLog = UserLogDto.CreateUserLog(input.UserId, input.Path, input.Method);

            // Câu lệnh SQL để chèn log vào bảng UserActivityLogs
            var sql = @"
                    INSERT INTO UserActivityLogs (UserId, Path, Method)
                    VALUES (@UserId, @Path, @Method)";

            // Thực thi câu lệnh SQL
            var result = await connection.ExecuteAsync(sql, new
            {
                userLog.UserId,
                userLog.Path,
                userLog.Method
            });

            // Trả về phản hồi thành công
            return ResponseText.ResponseSuccess("Thêm thành công.", StatusCodes.Status201Created);
        }

        public async Task<ResponseObject<PagedResult<UserLogDto>>> GetUserLogsAsync(int page, int limit)
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

            var userLogDtos = userLogs.Select(ul => new UserLogDto
            {
                UserId = ul.UserId,
                Path = ul.Path,
                Method = ul.Method,
                RoleName = ul.UserRole,
                UserEmail = ul.UserEmail,
                UserName = ul.UserName,
            }).ToList();

            var pagedResult = new PagedResult<UserLogDto>
            {
                TotalRecords = totalRecords,
                Page = page,
                Limit = limit,
                Data = userLogDtos
            };

            return new ResponseObject<PagedResult<UserLogDto>>(StatusCodes.Status200OK, "Lấy dữ liệu thành công.", pagedResult);
        }





    }
}
