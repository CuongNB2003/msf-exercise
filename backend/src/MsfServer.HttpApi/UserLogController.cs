using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MsfServer.Application.Contracts.UserLog;

namespace MsfServer.HttpApi
{
    [Route("api/log")]
    [ApiController]
    public class UserLogController(IUserLogRepository userLogRepository) : ControllerBase
    {
        private readonly IUserLogRepository _userLogRepository = userLogRepository;

        [Authorize(Roles = "admin")]
        [HttpGet] // lấy tất cả dữ liệu
        public async Task<IActionResult> GetUserLogs(int page, int limit)
        {
            return Ok(await _userLogRepository.GetUserLogsAsync(page, limit));
        }
    }
}
