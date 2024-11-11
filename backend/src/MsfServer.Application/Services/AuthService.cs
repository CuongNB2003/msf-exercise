using Dapper;
using Microsoft.AspNetCore.Http;
using MsfServer.Application.Contracts.Authentication;
using MsfServer.Application.Contracts.Authentication.AuthDto.InputDto;
using MsfServer.Application.Contracts.Authentication.AuthDto;
using MsfServer.Application.Contracts.ReCaptcha;
using MsfServer.Application.Contracts.Token;
using MsfServer.Application.Contracts.User;
using MsfServer.Application.Contracts.User.Dto;
using MsfServer.Application.Contracts.Dapper;
using MsfServer.Domain.Security;
using MsfServer.Domain.Shared.Exceptions;
using MsfServer.Domain.Shared.Responses;
using System.Data;
using Newtonsoft.Json;
using MsfServer.Application.Contracts.Role.Dto;

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
            //await _reCaptchaService.VerifyTokenAsync(input.ReCaptchaToken);
            //check email
            var user = await _userRepository.GetUserByEmailAsync(input.Email, input.PassWord);

            var userData = UserResponse.UserData(user.Id, user.Name!, user.Email!, user.Roles);
            // khởi tạo token
            var accessToken = await _tokenService.GenerateAccessTokenAsync(userData);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(userData);
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
            string hashedPassword = PasswordHashed.HashPassword(input.PassWord);
            var user = UserDto.CreateUserDto(input.Name, input.Email, hashedPassword, input.Avatar);
            var userJson = JsonConvert.SerializeObject(user);
            // Thêm người dùng
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var userId = await connection.ExecuteAsync(
                "User_Create",
                 new { UserJson = userJson },
                 commandType: CommandType.StoredProcedure
            );

            return ResponseText.ResponseSuccess("Đăng ký tài khoản thành công.", StatusCodes.Status201Created);
        }

        // lấy thông tin 
        public async Task<ResponseObject<UserLogin>> GetMeAsync(int id)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            // Thực hiện truy vấn hai lần
            using var multi = await connection.QueryMultipleAsync(
                "User_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );

            // Đọc thông tin người dùng
            var user = await multi.ReadSingleOrDefaultAsync<UserLogin>()
                        ?? throw new CustomException(StatusCodes.Status404NotFound, "Người dùng không tồn tại.");

            // Đọc danh sách vai trò
            var roles = await multi.ReadAsync<RoleDto>();
            user.Roles = roles.ToList(); // Gán danh sách vai trò cho user

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
