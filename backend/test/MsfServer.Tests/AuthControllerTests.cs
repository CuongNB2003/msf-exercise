using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MsfServer.Application.Contracts.Authentication;
using MsfServer.Application.Contracts.Authentication.AuthDtos;
using MsfServer.Application.Contracts.Authentication.AuthDtos.InputDtos;
using MsfServer.Application.Contracts.Log;
using MsfServer.Application.Contracts.User;
using MsfServer.Application.Contracts.User.UserDtos;
using MsfServer.Domain.Shared.Responses;
using System.Security.Claims;

namespace MsfServer.HttpApi.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ILogRepository> _userLogRepositoryMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _tokenServiceMock = new Mock<ITokenService>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _userLogRepositoryMock = new Mock<ILogRepository>();
            _controller = new AuthController(
                _authServiceMock.Object,
                _tokenServiceMock.Object,
                _userRepositoryMock.Object,
                _userLogRepositoryMock.Object
            );
        }

        [Fact] // Kiểm tra phương thức GetUser khi userId là null.
        public async Task GetUser_ReturnsBadRequest_WhenUserIdIsNull()
        {
            // giả lập dữ liệu
            var user = new ClaimsPrincipal(new ClaimsIdentity([], "mock"));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // gọi phương thức
            var result = await _controller.GetUser();

            // kết quả
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Id user bị null.", badRequestResult.Value);
        }


        [Fact] // Kiểm tra phương thức GetUser khi userId không hợp lệ.
        public async Task GetUser_ReturnsBadRequest_WhenUserIdIsInvalid()
        {
            // giả lập dữ liệu
            var user = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, "invalid")
            ], "mock"));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // gọi phương thức
            var result = await _controller.GetUser();

            // kết quả
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Id user không hợp lệ.", badRequestResult.Value);
        }

        [Fact] // Kiểm tra phương thức GetUser khi userId hợp lệ.
        public async Task GetUser_ReturnsOk_WhenUserIdIsValid()
        {
            // giả lập dữ liệu
            var user = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new(ClaimTypes.NameIdentifier, "1")
            ], "mock"));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
            var userEntity = new UserResultDto { Id = 1, Email = "testuser" };
            var responseObject = new ResponseObject<UserResultDto> { Data = userEntity, Status = 200, Message = "" };
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(1)).ReturnsAsync(responseObject);

            // gọi phương thức
            var result = await _controller.GetUser();

            // kết quả
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResponseObject = Assert.IsType<ResponseObject<UserResultDto>>(okResult.Value);
            Assert.Equal(userEntity, returnedResponseObject.Data);
        }


        [Fact] // Kiểm tra phương thức Login với thông tin đăng nhập hợp lệ.
        public async Task Login_ReturnsOk_WithValidCredentials()
        {
            // giả lập dữ liệu kiểm thử
            var loginInput = new LoginInputDto { Email = "testuser", PassWord = "password", ReCaptchaToken = "" };
            var loginResult = new LoginResultDto { User = null, Token = null, Expiration = DateTime.Now };
            var responseObject = new ResponseObject<LoginResultDto> { Data = loginResult, Status = 200, Message = "" };
            _authServiceMock.Setup(service => service.LoginAsync(loginInput)).ReturnsAsync(responseObject);

            // gọi phương thức
            var result = await _controller.Login(loginInput);

            // kết quả trả về 
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResponseObject = Assert.IsType<ResponseObject<LoginResultDto>>(okResult.Value);
            Assert.Equal(loginResult, returnedResponseObject.Data);
        }


        [Fact] // Kiểm tra phương thức Register với thông tin đăng ký hợp lệ.
        public async Task Register_ReturnsOk_WithValidInput()
        {
            // giả lập dữ liệu kiểm thử
            var registerInput = new RegisterInputDto { Email = "test@gmail.com", PassWord = "password", Avatar = "avatar", Name = "Test" };
            var responseText = new ResponseText { Message = "ok", Status = 201 };
            _authServiceMock.Setup(service => service.RegisterAsync(registerInput)).ReturnsAsync(responseText);

            // gọi phương thức
            var result = await _controller.Register(registerInput);

            // kết quả trả về 
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(responseText, okResult.Value);
        }

        [Fact] // Kiểm tra phương thức RefreshAccessToken với refresh token hợp lệ.
        public async Task RefreshAccessToken_ReturnsOk_WithValidToken()
        {
            // giả lập dữ liệu kiểm thử
            var refreshToken = "valid_refresh_token";
            var authTokenDto = new AuthTokenDto { AccessToken = null, RefreshToken = null };
            var responseObject = new ResponseObject<AuthTokenDto> { Data = authTokenDto, Status = 200, Message = "ok" };
            _tokenServiceMock.Setup(service => service.RefreshAccessTokenAsync(refreshToken)).ReturnsAsync(responseObject);


            // gọi phương thức
            var result = await _controller.RefreshAccessToken(refreshToken);

            // kết quả trả về 
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(responseObject, okResult.Value);
        }

        [Fact] // Kiểm tra phương thức Logout với userId hợp lệ.
        public async Task Logout_ReturnsOk_WithValidUserId()
        {
            // giả lập dữ liệu kiểm thử
            var user = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new(ClaimTypes.NameIdentifier, "1")
            ], "mock"));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
            var responseText = new ResponseText { Message = "ok", Status = 201 };
            _authServiceMock.Setup(service => service.LogoutAsync("1")).ReturnsAsync(responseText);

            // gọi phương thức
            var result = await _controller.Logout();

            // kết quả trả về 
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(responseText, okResult.Value);
        }
    }
}
