
namespace MsfServer.Application.Contracts.Permission.Dto
{
    public class PermissionResponse
    {
        public int Id { get; set; }
        public string? PermissionName { get; set; }
        public string? Description { get; set; }
        public int Total { get; set; }
        public int CountRole { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
