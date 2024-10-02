using System.ComponentModel.DataAnnotations;

namespace MsfServer.Application.Contracts.Authentication.AuthDtos.InputDtos
{
    public class ForgotPasswordInput
    {
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email chưa đúng định dạng.")]
        public string Email { get; set; } = string.Empty;
    }

}
