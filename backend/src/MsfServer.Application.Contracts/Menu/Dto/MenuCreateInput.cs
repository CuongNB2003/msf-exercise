
using System.ComponentModel.DataAnnotations;

namespace MsfServer.Application.Contracts.Menu.Dto
{
    public class MenuCreateInput
    {
        [Required(ErrorMessage = "Bạn cần phải nhập DisplayName.")]
        [MaxLength(100, ErrorMessage = "Không đc vượt quá 100 kí tự.")]
        public string? DisplayName { get; set; }
        [Required(ErrorMessage = "Bạn cần phải nhập Url.")]
        [MaxLength(100, ErrorMessage = "Không đc vượt quá 100 kí tự.")]
        public string? Url { get; set; }
        [Required(ErrorMessage = "Bạn cần phải nhập IconName.")]
        [MaxLength(50, ErrorMessage = "Không đc vượt quá 50 kí tự.")]
        public string? IconName { get; set; }
    }
}
