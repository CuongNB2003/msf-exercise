﻿using MsfServer.Application.Contracts.Role.RoleDtos;
using System.ComponentModel.DataAnnotations.Schema;

namespace MsfServer.Application.Contracts.User.UserDtos
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int RoleId { get; set; }
        public string? Avatar { get; set; }
        public DateTime? CreatedAt { get; set; }

        [ForeignKey("RoleId")]
        public RoleDto? Role { get; set; }
    }
}