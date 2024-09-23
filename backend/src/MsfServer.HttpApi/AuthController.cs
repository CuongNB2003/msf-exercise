﻿using Microsoft.AspNetCore.Mvc;
using MsfServer.Application.Contracts.Authentication;
using MsfServer.Application.Contracts.Authentication.AuthDtos;
using MsfServer.Application.Contracts.Users;
using MsfServer.Domain.Shared.Responses;
using System.Security.Claims;

namespace MsfServer.HttpApi
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService, ITokenService tokenService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ITokenService _tokenService = tokenService;

        //[HttpGet("me")]
        //public async Task<IActionResult> GetUser()
        //{
        //    return Ok();
        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginInputDto loginInput)
        {
            var result = await _authService.LoginAsync(loginInput);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshAccessToken(string refreshToken)
        {
            var result = await _tokenService.RefreshAccessTokenAsync(refreshToken);
            return Ok(result);
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register(RegisterInputDto registerInput)
        //{
        //    return Ok();
        //}

        //[HttpPost("logout")]
        //public async Task<IActionResult> Logout()
        //{
        //    return Ok();
        //}

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
