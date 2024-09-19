using Microsoft.EntityFrameworkCore;
using MsfServer.Domain.users;

namespace MsfServer.DbMigrator
{
    public class MsfServerDbContext : DbContext
    {
        public MsfServerDbContext(DbContextOptions<MsfServerDbContext> options)
            : base(options)
        {
        }

        // Định nghĩa các DbSet cho các bảng trong cơ sở dữ liệu
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Cấu hình thêm nếu cần
        }
    }
}
