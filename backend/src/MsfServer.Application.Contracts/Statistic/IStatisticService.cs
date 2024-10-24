
using MsfServer.Application.Contracts.Statistic.Dto;
using MsfServer.Domain.Shared.Responses;

namespace MsfServer.Application.Contracts.Statistic
{
    public interface IStatisticService
    {
        Task<ResponseObject<IEnumerable<StatisticLogMethodByYear>>> GetLogMethodByYearAsync(DateTime startDate, DateTime endDate);
        Task<ResponseObject<IEnumerable<StatisticRoleCountUser>>> GetRoleCountUserAsync();
        Task<ResponseObject<IEnumerable<StatisticLogMethodByMonth>>> GetLogMethodByMonthAsync(DateTime searchDate);

    }
}
