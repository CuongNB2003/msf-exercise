using Microsoft.EntityFrameworkCore;
using MsfServer.Domain.roles;
using MsfServer.Domain.users;

namespace MsfServer.EntityFrameworkCore
{
    public class MsfServerDbContext(DbContextOptions<MsfServerDbContext> options) : DbContext(options)
    {

        // Định nghĩa các DbSet cho các bảng trong cơ sở dữ liệu
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Cấu hình thêm nếu cần

        }
    }
}
