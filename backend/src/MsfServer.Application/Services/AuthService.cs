using Dapper;
using Microsoft.AspNetCore.Http;
using MsfServer.Application.Contracts.Authentication;
using MsfServer.Application.Contracts.Authentication.AuthDto.InputDto;
using MsfServer.Application.Contracts.Authentication.AuthDto;
using MsfServer.Application.Contracts.ReCaptcha;
using MsfServer.Application.Contracts.Role.Dto;
using MsfServer.Application.Contracts.Token;
using MsfServer.Application.Contracts.User;
using MsfServer.Application.Contracts.User.Dto;
using MsfServer.Application.Dapper;
using MsfServer.Domain.Security;
using MsfServer.Domain.Shared.Exceptions;
using MsfServer.Domain.Shared.Responses;
using System.Data;

namespace MsfServer.Application.Services
{
    public class AuthService(
        IReCaptchaService reCaptchaService,
        IUserRepository userRepository,
        ITokenService tokenService,
        ITokenRepository tokenRepository,
        string connectionString
        ) : IAuthService
    {
        private readonly IReCaptchaService _reCaptchaService = reCaptchaService;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITokenService _tokenService = tokenService;
        private readonly ITokenRepository _tokenRepository = tokenRepository;
        private readonly string _connectionString = connectionString;

        // đăng nhập
        public async Task<ResponseObject<LoginResponse>> LoginAsync(LoginInput input)
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
            var userLogin = UserLogin.FromUserDto(user);
            var token = TokenLogin.GetToken(accessToken, refreshToken);
            var result = LoginResponse.CreateResult(token, userLogin);

            return ResponseObject<LoginResponse>.CreateResponse("Đăng nhậpthành công.", result);
        }
        // đăng xuất
        public async Task<ResponseText> LogoutAsync(string userId)
        {
            await _tokenRepository.DeleteTokenAsync(userId);
            return ResponseText.ResponseSuccess("Đăng xuất thành công.", StatusCodes.Status200OK);
        }
        // đăng ký
        public async Task<ResponseText> RegisterAsync(RegisterInput input)
        {
            if (await _userRepository.CheckEmailExistsAsync(input.Email))
            {
                throw new CustomException(StatusCodes.Status409Conflict, "Email đã tồn tại.");
            }

            // Tạo dữ liệu
            byte[] salt = PasswordHashed.GenerateSalt();
            string hashedPassword = PasswordHashed.HashPassword(input.PassWord, salt);
            var user = UserDto.CreateUserDto(input.Name, input.Email, hashedPassword, 3, input.Avatar, salt);

            // Thêm người dùng
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var userId = await connection.ExecuteAsync(
                "User_Insert",
                 new
                 {
                     user.Name,
                     user.Email,
                     user.Password,
                     user.RoleId,
                     user.Avatar,
                     user.Salt
                 },
                     commandType: CommandType.StoredProcedure
            );

            return ResponseText.ResponseSuccess("Đăng ký tài khoản thành công.", StatusCodes.Status201Created);
        }

        // lấy thông tin 
        public async Task<ResponseObject<UserLogin>> GetMeAsync(int IdUser)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var sql = @"
                    SELECT * FROM Users WHERE Id = @Id;
                    SELECT * FROM Roles WHERE Id = (SELECT RoleId FROM Users WHERE Id = @Id);";

            using var multi = await connection.QueryMultipleAsync(sql, new { Id = IdUser });

            var user = await multi.ReadSingleOrDefaultAsync<UserLogin>();
            var role = await multi.ReadSingleOrDefaultAsync<RoleDto>();

            if (user == null)
            {
                throw new CustomException(StatusCodes.Status404NotFound, "Không tìm thấy User.");
            }
            if (role == null)
            {
                throw new CustomException(StatusCodes.Status404NotFound, "Không tìm thấy Role.");
            }
            user.Role = role;
            return ResponseObject<UserLogin>.CreateResponse("Lấy dữ liệu thành công.", user);
        }

        // đổi mật khẩu
        public Task<ResponseText> ChangePasswordAsync(ChangePasswordInput input)
        {
            throw new NotImplementedException();
        }
        // quên mật khẩu
        public Task<ResponseText> ForgotPasswordAsync(ForgotPasswordInput input)
        {
            throw new NotImplementedException();
        }
        // đặt lại mật khẩu
        public Task<ResponseText> ResetPasswordAsync(ResetPasswordInput input)
        {
            throw new NotImplementedException();
        }
    }
}
