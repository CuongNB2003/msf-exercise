using MsfServer.Application.Contracts.Role.Dto;
using System.ComponentModel.DataAnnotations.Schema;

namespace MsfServer.Application.Contracts.User.Dto
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int RoleId { get; set; }
        public string? Avatar { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int TotalUser { get; set; }

        [ForeignKey("RoleId")]
        public RoleDto? Role { get; set; }

        public static UserResponse UserData(int id, string name, string email, RoleDto role, int roleId )
        {
            return new UserResponse
            {
                Id = id,
                Name = name,
                Email = email,
                RoleId = roleId,
                Role = role,
            };
        }
    }
}
