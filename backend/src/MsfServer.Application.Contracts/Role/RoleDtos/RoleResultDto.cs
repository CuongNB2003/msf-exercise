﻿
namespace MsfServer.Application.Contracts.Role.RoleDtos
{
    public class RoleResultDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int CountUser { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
