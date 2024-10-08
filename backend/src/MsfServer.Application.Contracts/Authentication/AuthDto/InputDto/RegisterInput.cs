using System.ComponentModel.DataAnnotations;

namespace MsfServer.Application.Contracts.Authentication.AuthDto.InputDto
{
    public class RegisterInput
    {
        [Required(ErrorMessage = "Name là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Vượt quá kí tự cho phép.")]
        public string Name { get; set; } = string.Empty;


        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email chưa đúng định dạng.")]
        [MaxLength(50, ErrorMessage = "Vượt quá kí tự cho phép.")]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = "PassWord là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Vượt quá kí tự cho phép.")]
        public string PassWord { get; set; } = string.Empty;

        [MaxLength(255, ErrorMessage = "Vượt quá kí tự cho phép.")]
        public string Avatar { get; set; } = string.Empty;
    }
}
