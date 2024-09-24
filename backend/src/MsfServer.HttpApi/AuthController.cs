using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MsfServer.Application.Contracts.Authentication;
using MsfServer.Application.Contracts.Authentication.AuthDtos.InputDtos;
using MsfServer.Application.Contracts.User;
using MsfServer.Application.Contracts.UserLog;
using MsfServer.Application.Contracts.UserLog.UserLogDtos;
using MsfServer.Domain.Shared.Exceptions;
using MsfServer.Domain.Shared.Responses;
using MsfServer.HttpApi.Helper;
using System.Security.Claims;

namespace MsfServer.HttpApi
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService, ITokenService tokenService, IUserRepository userRepository, IUserLogRepository userLogRepository) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IUserRepository _userRepository = userRepository; 
        private readonly IUserLogRepository _userLogRepository = userLogRepository;

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null)
            {
                return BadRequest("Id user bị null.");
            }

            if (!int.TryParse(userIdString, out int userId))
            {
                return BadRequest("Id user không hợp lệ.");
            }

            var user = await _userRepository.GetUserByIdAsync(userId);
            return Ok(user);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginInputDto loginInput)
        {
            var result = await _authService.LoginAsync(loginInput);
            var token = result.Data!.Token!.AccessToken!.Token;
            int idUser = TokenHelper.GetUserIdFromToken(token!);
            var path = HttpContext.Request.Path;
            var method = HttpContext.Request.Method;
            var userLog = UserLogDto.CreateUserLog(idUser, path, method);
            await _userLogRepository.CreateUserLogAsync(userLog);

            return Ok(result);
        }

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<IActionResult> RefreshAccessToken(string refreshToken)
        {
            return Ok(await _tokenService.RefreshAccessTokenAsync(refreshToken));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterInputDto registerInput)
        {
            var result = await _authService.RegisterAsync(registerInput);
            var path = HttpContext.Request.Path;
            var method = HttpContext.Request.Method;
            if (!int.TryParse(result.Message, out int userId))
            {
                throw new CustomException(StatusCodes.Status500InternalServerError, "Không thể chuyển đổi message thành số.");
            }
            var userLog = UserLogDto.CreateUserLog(userId, path, method);
            await _userLogRepository.CreateUserLogAsync(userLog);

            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
            {
                return BadRequest("Id user bị null.");
            }
            return Ok(await _authService.LogoutAsync(userId!));
        }

        //[HttpPost("change-password")]
        //public async Task<IActionResult> ChangePassword(ChangePasswordInputDto input)
        //{
        //    return Ok();
        //}

        //[HttpPost("forgot-password")]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordInputDto input)
        //{
        //    return Ok();
        //}

        //[HttpPost("reset-password")]
        //public async Task<IActionResult> ResetPassword(ResetPasswordInputDto input)
        //{
        //    return Ok();
        //}

    }
}
