using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MsfServer.Application.Contracts.Authentication;
using MsfServer.Application.Contracts.Authentication.AuthDto.InputDto;
using System.Security.Claims;

namespace MsfServer.HttpApi
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService, ITokenService tokenService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ITokenService _tokenService = tokenService;

        [HttpGet("me")]
        [Authorize(Policy = "PermissionPolicy")]
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

            var user = await _authService.GetMeAsync(userId);
            return Ok(user);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginInput loginInput)
        {
            var result = await _authService.LoginAsync(loginInput);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        //[Authorize]
        public async Task<IActionResult> RefreshAccessToken(string refreshToken)
        {
            return Ok(await _tokenService.RefreshAccessTokenAsync(refreshToken));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterInput registerInput)
        {
            var result = await _authService.RegisterAsync(registerInput);
            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize(Policy = "PermissionPolicy")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
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
