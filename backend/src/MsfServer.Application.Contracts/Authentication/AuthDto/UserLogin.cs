using MsfServer.Application.Contracts.Role.Dto;
using MsfServer.Application.Contracts.User.Dto;
using System.ComponentModel.DataAnnotations.Schema;

namespace MsfServer.Application.Contracts.Authentication.AuthDto
{
    public class UserLogin
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int RoleId { get; set; }
        public string? Avatar { get; set; }
        [ForeignKey("RoleId")]
        public RoleDto? Role { get; set; }

        public static UserLogin FromUserDto(UserDto user)
        {
            return new UserLogin
            {
                Name = user.Name,
                Email = user.Email,
                Avatar = user.Avatar,
                RoleId = user.RoleId,
                Role = user.Role
            };
        }
    }
}
