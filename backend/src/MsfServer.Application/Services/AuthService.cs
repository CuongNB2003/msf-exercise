using Microsoft.AspNetCore.Http;
using MsfServer.Application.Contracts.Authentication;
using MsfServer.Application.Contracts.Authentication.AuthDtos;
using MsfServer.Application.Contracts.Services;
using MsfServer.Application.Contracts.Users;
using MsfServer.Application.Contracts.Users.UserDtos;
using MsfServer.Domain.Security;
using MsfServer.Domain.Shared.Exceptions;
using MsfServer.Domain.Shared.Responses;
using MsfServer.Domain.users;

namespace MsfServer.Application.Services
{
    public class AuthService(
        IReCaptchaService reCaptchaService, 
        IUserRepository userRepository, 
        ResponseObject<LoginResultDto> response, 
        string connectionString, 
        ITokenService tokenService
        ) : IAuthService
    {
        private readonly IReCaptchaService _reCaptchaService = reCaptchaService;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ResponseObject<LoginResultDto> _response = response;
        private readonly string _connectionString = connectionString;
        private readonly ITokenService _tokenService = tokenService;

        // đăng nhập
        public async Task<ResponseObject<LoginResultDto>> LoginAsync(LoginInputDto input)
        {
            //if (!await _reCaptchaService.VerifyTokenAsync(input.ReCaptchaToken))
            //{
            //    throw new CustomException(StatusCodes.Status400BadRequest, "ReCAPTCHA token không hợp lệ.");
            //}
            //check email
            var user = await _userRepository.GetUserByEmailAsync(input.Email);
            // check password
            byte[] salt = Convert.FromBase64String(user.Salt!);
            if (!PasswordHashed.VerifyPassword(input.PassWord, user.Password!, salt))
            {
                throw new CustomException(StatusCodes.Status401Unauthorized, "Sai mật khẩu.");
            }
            // khởi tạo token
            var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user);
            // khởi tạo user
            var userLogin = UserLoginDto.FromUserDto(user);
            var result = LoginResultDto.CreateResult(accessToken, refreshToken, userLogin);
            return _response.ResponseSuccess("Đăng nhập thành công thành công.", result);
        }
        // đăng xuất
        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }
        // đăng ký
        public Task<ResponseText> RegisterAsync(RegisterInputDto input)
        {
            throw new NotImplementedException();
        }
        // đổi mật khẩu
        public Task<ResponseText> ChangePasswordAsync(ChangePasswordInputDto input)
        {
            throw new NotImplementedException();
        }
        // quên mật khẩu
        public Task<ResponseText> ForgotPasswordAsync(ForgotPasswordInputDto input)
        {
            throw new NotImplementedException();
        }
        // lấy thông tin 
        public Task<ResponseObject<UserResultDto>> GetUserAsync()
        {
            throw new NotImplementedException();
        }
        // đặt lại mật khẩu
        public Task<ResponseText> ResetPasswordAsync(ResetPasswordInputDto input)
        {
            throw new NotImplementedException();
        }
    }
}
