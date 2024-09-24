using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MsfServer.Application.Contracts.Authentication.AuthDtos
{
    public class LoginInputDto
    {
        [Required(ErrorMessage = "Không được để trống Email")]
        [EmailAddress(ErrorMessage ="Email chưa đúng định dạng")]
        [DefaultValue("cuong@gmail.com")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Không được để trống PassWord")]
        [MinLength(6,ErrorMessage = "PassWord tối thiểu 6 kí tự")]
        [DefaultValue("111111")]
        public string PassWord { get; set; } = string.Empty;

        [Required(ErrorMessage = "Không được để trống ReCaptchaToken")]
        public string ReCaptchaToken { get; set; } = string.Empty;
    }
}
