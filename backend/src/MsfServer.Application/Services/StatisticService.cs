using Dapper;
using MsfServer.Application.Contracts.Statistic;
using MsfServer.Application.Contracts.Statistic.Dto;
using MsfServer.Domain.Shared.Responses;
using System.Data;
using MsfServer.Application.Contracts.Dapper;

namespace MsfServer.Application.Services
{
    public class StatisticService(string connectionString) : IStatisticService
    {
        private readonly string _connectionString = connectionString;

        public async Task<ResponseObject<IEnumerable<LogMethodStatistics>>> GetLogStatisticsAsync(DateTime startDate, DateTime endDate)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@StartDate", startDate);
            parameters.Add("@EndDate", endDate);

            var result = await connection.QueryAsync<LogMethodStatistics>(
                "[dbo].[Statistic_LogStatus]",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return ResponseObject<IEnumerable<LogMethodStatistics>>.CreateResponse("Lấy dữ liệu thành công", result);
        }

        public async Task<ResponseObject<IEnumerable<RoleCountUserStatistics>>> GetRoleStatisticsAsync()
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();

            var result = await connection.QueryAsync<RoleCountUserStatistics>(
                "[dbo].[Statistic_RoleCountUser]",
                commandType: CommandType.StoredProcedure
            );

            return ResponseObject<IEnumerable<RoleCountUserStatistics>>.CreateResponse("Lấy dữ liệu thành công", result);
        }
    }
}
