using MsfServer.Application.Contracts.Role.Dto;
using MsfServer.Application.Contracts.User.Dto;

namespace MsfServer.Application.Contracts.Authentication.AuthDto
{
    public class UserLogin
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Avatar { get; set; }

        public List<RoleDto> Roles { get; set; } = [];

        public static UserLogin FromUserDto(UserDto user)
        {
            return new UserLogin
            {
                Name = user.Name,
                Email = user.Email,
                Avatar = user.Avatar,
                Roles = user.Roles
            };
        }
    }
}
