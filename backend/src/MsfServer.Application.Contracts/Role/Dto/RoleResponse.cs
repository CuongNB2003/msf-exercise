using MsfServer.Application.Contracts.Menu.Dto;
using MsfServer.Application.Contracts.Permission.Dto;

namespace MsfServer.Application.Contracts.Role.Dto
{
    public class RoleResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int CountUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Total { get; set; }
        public List<MenuResponse> Menus { get; set; } = [];
        public List<PermissionResponse> Permissions { get; set; } = [];
    }
}
