namespace MsfServer.Application.Contracts.ReCaptcha
{
    public interface IReCaptchaService
    {
        Task<bool> VerifyTokenAsync(string recaptcha);
    }
}
