
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MsfServer.Application.Contracts.Statistic;

namespace MsfServer.HttpApi
{
    [Route("api/statistics")]
    [ApiController]
    //[Authorize(Policy = "PermissionPolicy")]
    public class StatisticsController(IStatisticService statisticService) : ControllerBase
    {
        private readonly IStatisticService _statisticService = statisticService;

        [HttpGet("log-method-by-year")] // lấy tất cả dữ liệu
        [Authorize(Policy = "PermissionPolicy")]
        public async Task<IActionResult> GetLogMethodByYear(DateTime startDate, DateTime endDate)
        {
            return Ok(await _statisticService.GetLogMethodByYearAsync(startDate, endDate));
        }

        [HttpGet("log-method-by-month")] // lấy tất cả dữ liệu
        [Authorize(Policy = "PermissionPolicy")]
        public async Task<IActionResult> GetLogMethodByMonth(DateTime searchDate)
        {
            return Ok(await _statisticService.GetLogMethodByMonthAsync(searchDate));
        }

        [HttpGet("role-count-user")] // lấy tất cả dữ liệu
        [Authorize(Policy = "PermissionPolicy")]
        public async Task<IActionResult> GetRoleCountUser()
        {
            return Ok(await _statisticService.GetRoleCountUserAsync());
        }
    }
}
