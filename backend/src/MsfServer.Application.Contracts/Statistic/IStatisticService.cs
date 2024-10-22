
using MsfServer.Application.Contracts.Statistic.Dto;
using MsfServer.Domain.Shared.Responses;

namespace MsfServer.Application.Contracts.Statistic
{
    public interface IStatisticService
    {
        Task<ResponseObject<IEnumerable<LogMethodStatistics>>> GetLogStatisticsAsync(DateTime startDate, DateTime endDate);

        Task<ResponseObject<IEnumerable<RoleCountUserStatistics>>> GetRoleStatisticsAsync();
    }
}
