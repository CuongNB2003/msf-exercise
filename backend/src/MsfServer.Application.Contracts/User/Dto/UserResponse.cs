using MsfServer.Application.Contracts.Role.Dto;
using Newtonsoft.Json;

namespace MsfServer.Application.Contracts.User.Dto
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Avatar { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int Total { get; set; }

        public List<RoleDto> Roles { get; set; } = [];
        public static UserResponse UserData(int id, string name, string email)
        {
            return new UserResponse
            {
                Id = id,
                Name = name,
                Email = email,
            };
        }
    }
}
