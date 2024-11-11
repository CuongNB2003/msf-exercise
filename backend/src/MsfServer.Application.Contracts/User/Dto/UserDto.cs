using MsfServer.Application.Contracts.Role.Dto;
using System.ComponentModel.DataAnnotations.Schema;

namespace MsfServer.Application.Contracts.User.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Avatar { get; set; }
        public List<int> RoleIds { get; set; } = [];
        public List<RoleDto> Roles { get; set; } = [];

        public static UserDto CreateUserAdminDto(string email, string hashedPassword, string avatar, List<int> roleIds)
        {
            var nameFromEmail = email.Split('@')[0];
            return new UserDto
            {
                Name = nameFromEmail,
                Email = email,
                Password = hashedPassword,
                Avatar = avatar,
                RoleIds = roleIds
            };
        }

        public static UserDto CreateUserDto(string name, string email, string hashedPassword, string avatar)
        {
            return new UserDto
            {
                Name = name,
                Email = email,
                Password = hashedPassword,
                Avatar = avatar
            };
        }
    }
}
