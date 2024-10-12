
namespace MsfServer.Domain.Entities
{
    public class RolePermissionEntity : BaseEntity
    {
        public int? RoleId { get; set; }  // FK tới Role
        public RoleEntity? Role { get; set; }   // Điều hướng Role

        public int? PermissionId { get; set; }  // FK tới Permission
        public PermissionEntity? Permission { get; set; }  // Điều hướng Permission
    }
}
