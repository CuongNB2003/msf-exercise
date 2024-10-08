﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MsfServer.Application.Contracts.Log;

namespace MsfServer.HttpApi
{
    [Route("api/log")]
    [ApiController]
    public class LogsController(ILogRepository logRepository) : ControllerBase
    {
        private readonly ILogRepository _logRepository = logRepository;

        [HttpGet] // lấy tất cả dữ liệu
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> GetLogs(int page, int limit)
        {
            return Ok(await _logRepository.GetLogsAsync(page, limit));
        }

        [HttpGet("{id}")]  // lấy theo id
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetLog(int id)
        {
            return Ok(await _logRepository.GetLogByIdAsync(id));
        }
    }
}
