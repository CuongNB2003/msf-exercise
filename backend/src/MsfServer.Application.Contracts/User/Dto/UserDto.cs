﻿using MsfServer.Application.Contracts.Role.Dto;
using System.ComponentModel.DataAnnotations.Schema;

namespace MsfServer.Application.Contracts.User.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Salt { get; set; }
        public int RoleId { get; set; }
        public string? Avatar { get; set; }
        [ForeignKey("RoleId")]
        public RoleDto? Role { get; set; }

        public static UserDto CreateUserAdminDto(string email, string hashedPassword, int roleId, string avatar, byte[] salt)
        {
            var nameFromEmail = email.Split('@')[0];
            return new UserDto
            {
                Name = nameFromEmail,
                Email = email,
                Password = hashedPassword,
                RoleId = roleId,
                Avatar = avatar,
                Salt = Convert.ToBase64String(salt)
            };
        }

        public static UserDto CreateUserDto(string name, string email, string hashedPassword, int roleId, string avatar, byte[] salt)
        {
            return new UserDto
            {
                Name = name,
                Email = email,
                Password = hashedPassword,
                RoleId = roleId,
                Avatar = avatar,
                Salt = Convert.ToBase64String(salt)
            };
        }
    }
}
