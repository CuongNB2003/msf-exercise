using MsfServer.Application.Contracts.Roles.RoleDtos;
using MsfServer.Application.Contracts.User.UserDtos;
using System.ComponentModel.DataAnnotations.Schema;

namespace MsfServer.Application.Contracts.Authentication.AuthDtos
{
    public class UserLoginDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int RoleId { get; set; }
        public string? Avatar { get; set; }
        [ForeignKey("RoleId")]
        public required RoleResultDto Role { get; set; }

        public static UserLoginDto FromUserDto(UserDto user)
        {
            return new UserLoginDto
            {
                Name = user.Name,
                Email = user.Email,
                Avatar = user.Avatar,
                RoleId = user.RoleId,
                Role = user.Role!
            };
        }
    }
}
