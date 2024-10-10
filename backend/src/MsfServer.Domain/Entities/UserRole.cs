
namespace MsfServer.Domain.Entities
{
    public class UserRole : BaseEntity
    {
        public int UserId { get; set; }  // FK tới User
        public User? User { get; set; }   // Điều hướng User

        public int RoleId { get; set; }  // FK tới Role
        public Role? Role { get; set; }   // Điều hướng Role
    }
}
