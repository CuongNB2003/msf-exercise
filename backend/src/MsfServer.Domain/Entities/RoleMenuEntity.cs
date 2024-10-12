
namespace MsfServer.Domain.Entities
{
    public class RoleMenuEntity  : BaseEntity
    {
        public int RoleId { get; set; }  // FK tới Role
        public RoleEntity? Role { get; set; }   // Điều hướng Role

        public int MenuId { get; set; }  // FK tới Menu
        public MenuEntity? Menu { get; set; }   // Điều hướng Menu
    }
}
