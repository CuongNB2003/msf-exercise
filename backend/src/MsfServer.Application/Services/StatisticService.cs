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

        public async Task<ResponseObject<IEnumerable<StatisticLogMethodByMonth>>> GetLogMethodByMonthAsync(DateTime searchDate)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@InputDate", searchDate);

            var result = await connection.QueryAsync<StatisticLogMethodByMonth>(
                "[dbo].[Get_LogMethodByMonth]",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return ResponseObject<IEnumerable<StatisticLogMethodByMonth>>.CreateResponse("Lấy dữ liệu thành công", result);
        }

        public async Task<ResponseObject<IEnumerable<StatisticLogMethodByYear>>> GetLogMethodByYearAsync(DateTime startDate, DateTime endDate)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@StartDate", startDate);
            parameters.Add("@EndDate", endDate);

            var result = await connection.QueryAsync<StatisticLogMethodByYear>(
                "[dbo].[Get_LogMethodByYear]",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return ResponseObject<IEnumerable<StatisticLogMethodByYear>>.CreateResponse("Lấy dữ liệu thành công", result);
        }

        public async Task<ResponseObject<IEnumerable<StatisticRoleCountUser>>> GetRoleCountUserAsync()
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();

            var result = await connection.QueryAsync<StatisticRoleCountUser>(
                "[dbo].[Get_RoleCountUser]",
                commandType: CommandType.StoredProcedure
            );

            return ResponseObject<IEnumerable<StatisticRoleCountUser>>.CreateResponse("Lấy dữ liệu thành công", result);
        }
    }
}
