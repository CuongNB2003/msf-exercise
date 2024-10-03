
using MsfServer.Application.Contracts.Authentication.AuthDtos;
using MsfServer.Application.Contracts.Authentication.AuthDtos.InputDtos;
using MsfServer.Domain.Shared.Responses;

namespace MsfServer.Application.Contracts.Authentication
{
    public interface IAuthService
    {
        Task<ResponseObject<LoginResponse>> LoginAsync(LoginInput input);
        Task<ResponseObject<UserLogin>> GetMeAsync(int IdUser);
        Task<ResponseText> RegisterAsync(RegisterInput input);
        Task<ResponseText> LogoutAsync(string userId);
        Task<ResponseText> ChangePasswordAsync(ChangePasswordInput input);
        Task<ResponseText> ForgotPasswordAsync(ForgotPasswordInput input);
        Task<ResponseText> ResetPasswordAsync(ResetPasswordInput input);
    }
}
