using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MsfServer.Application.Contracts.Log;
using MsfServer.HttpApi.Sercurity;

namespace MsfServer.HttpApi
{
    [Route("api/log")]
    [ApiController]
    [Authorize(Policy = "PermissionPolicy")]
    public class LogsController(ILogRepository logRepository) : ControllerBase
    {
        private readonly ILogRepository _logRepository = logRepository;

        [HttpGet] // lấy tất cả dữ liệu
        [AuthorizePermission("5")]
        public async Task<IActionResult> GetLogs(int page, int limit)
        {
            return Ok(await _logRepository.GetLogsAsync(page, limit));
        }

        [HttpGet("{id}")]  // lấy theo id
        public async Task<IActionResult> GetLog(int id)
        {
            return Ok(await _logRepository.GetLogByIdAsync(id));
        }
    }
}
