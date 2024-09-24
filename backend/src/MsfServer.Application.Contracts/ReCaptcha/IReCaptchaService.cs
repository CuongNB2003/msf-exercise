
namespace MsfServer.Application.Contracts.Services
{
    public interface IReCaptchaService
    {
        Task<bool> VerifyTokenAsync(string recaptcha);
    }
}
