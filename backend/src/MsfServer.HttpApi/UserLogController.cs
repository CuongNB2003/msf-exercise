using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MsfServer.Application.Contracts.Log;

namespace MsfServer.HttpApi
{
    [Route("api/log")]
    [ApiController]
    public class UserLogController(ILogRepository userLogRepository) : ControllerBase
    {
        private readonly ILogRepository _userLogRepository = userLogRepository;

        [Authorize(Roles = "admin")]
        [HttpGet] // lấy tất cả dữ liệu
        public async Task<IActionResult> GetUserLogs(int page, int limit)
        {
            return Ok(await _userLogRepository.GetLogsAsync(page, limit));
        }
    }
}
