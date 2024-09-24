
using MsfServer.Application.Contracts.Authentication.AuthDtos;
using MsfServer.Application.Contracts.Users.UserDtos;
using MsfServer.Domain.Shared.Responses;

namespace MsfServer.Application.Contracts.Authentication
{
    public interface IAuthService
    {
        Task<ResponseObject<LoginResultDto>> LoginAsync(LoginInputDto input);
        Task<ResponseText> RegisterAsync(RegisterInputDto input);
        Task<ResponseText> LogoutAsync(string userId);
        Task<ResponseText> ChangePasswordAsync(ChangePasswordInputDto input);
        Task<ResponseText> ForgotPasswordAsync(ForgotPasswordInputDto input);
        Task<ResponseText> ResetPasswordAsync(ResetPasswordInputDto input);
        Task<ResponseObject<UserResultDto>> GetUserAsync();
    }
}
