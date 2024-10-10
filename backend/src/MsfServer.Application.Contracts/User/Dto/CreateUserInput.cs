using System.ComponentModel.DataAnnotations;

namespace MsfServer.Application.Contracts.User.Dto
{
    public class CreateUserInput
    {
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email chưa đúng định dạng.")]
        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Avatar { get; set; } = string.Empty;
    }
}
