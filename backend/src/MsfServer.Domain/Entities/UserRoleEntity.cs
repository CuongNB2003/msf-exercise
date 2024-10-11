
namespace MsfServer.Domain.Entities
{
    public class UserRoleEntity : BaseEntity
    {
        public int UserId { get; set; }  // FK tới User
        public UserEntity? User { get; set; }   // Điều hướng User

        public int RoleId { get; set; }  // FK tới Role
        public RoleEntity? Role { get; set; }   // Điều hướng Role
    }
}
