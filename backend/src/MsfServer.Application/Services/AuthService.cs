using MsfServer.Application.Contracts.Authentication;
using MsfServer.Application.Contracts.Authentication.AuthDtos;
using MsfServer.Application.Contracts.Users.UserDtos;
using MsfServer.Domain.Shared.Responses;

namespace MsfServer.Application.Services
{
    public class AuthService : IAuthService
    {
        // đăng nhập
        public Task<ResponseObject<LoginResultDto>> LoginAsync(LoginInputDto input)
        {
            throw new NotImplementedException();
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
