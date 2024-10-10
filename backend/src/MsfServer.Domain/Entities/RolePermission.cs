
namespace MsfServer.Domain.Entities
{
    public class RolePermission : BaseEntity
    {
        public int? RoleId { get; set; }  // FK tới Role
        public Role? Role { get; set; }   // Điều hướng Role

        public int? PermissionId { get; set; }  // FK tới Permission
        public Permission? Permission { get; set; }  // Điều hướng Permission
    }
}
