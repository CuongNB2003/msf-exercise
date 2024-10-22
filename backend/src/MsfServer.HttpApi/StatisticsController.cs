
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MsfServer.Application.Contracts.Log;
using MsfServer.Application.Contracts.Statistic;
using MsfServer.HttpApi.Sercurity;

namespace MsfServer.HttpApi
{
    [Route("api/statistics")]
    [ApiController]
    //[Authorize(Policy = "PermissionPolicy")]
    public class StatisticsController(IStatisticService statisticService) : ControllerBase
    {
        private readonly IStatisticService _statisticService = statisticService;

        [HttpGet("log-method")] // lấy tất cả dữ liệu
        //[AuthorizePermission(AuthorPermission.Log.View)]
        public async Task<IActionResult> GetLogMethod(DateTime startDate, DateTime endDate)
        {
            return Ok(await _statisticService.GetLogStatisticsAsync(startDate, endDate));
        }

        [HttpGet("role-count-user")] // lấy tất cả dữ liệu
        //[AuthorizePermission(AuthorPermission.Log.View)]
        public async Task<IActionResult> GetRoleCountUser()
        {
            return Ok(await _statisticService.GetRoleStatisticsAsync());
        }
    }
}
