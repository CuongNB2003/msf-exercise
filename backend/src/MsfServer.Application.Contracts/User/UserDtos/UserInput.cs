﻿using System.ComponentModel.DataAnnotations;

namespace MsfServer.Application.Contracts.Users.UserDtos
{
    public class UserInput
    {
        [Required(ErrorMessage = "Name là bắt buộc.")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email chưa đúng định dạng.")]
        [MaxLength(50)]
        public string Email { get; set; } = string.Empty; 

        [Required(ErrorMessage = "RoleId là bắt buộc.")]
        public int RoleId { get; set; }

        [MaxLength(255)]
        public string Avatar { get; set; } = string.Empty;
    }
}