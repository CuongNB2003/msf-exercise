using Microsoft.EntityFrameworkCore;
using MsfServer.Domain.Entities;

namespace MsfServer.EntityFrameworkCore.Database
{
    public class MsfServerDbContext(DbContextOptions<MsfServerDbContext> options) : DbContext(options)
    {

        // Định nghĩa các DbSet cho các bảng trong cơ sở dữ liệu
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<TokenEntity> Tokens { get; set; }
        public DbSet<LogEntity> Logs { get; set; }
        public DbSet<MenuEntity> Menu { get; set; }
        public DbSet<RolePermissionEntity> Role_Permission { get; set; }
        public DbSet<UserRoleEntity> User_Role { get; set; }
        public DbSet<RoleMenuEntity> Role_Menu { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Gọi phương thức mở rộng cho từng thực thể
            modelBuilder.ApplyBaseEntityConfiguration<RoleEntity>();
            modelBuilder.ApplyBaseEntityConfiguration<PermissionEntity>();
            modelBuilder.ApplyBaseEntityConfiguration<UserEntity>();
            modelBuilder.ApplyBaseEntityConfiguration<TokenEntity>();
            modelBuilder.ApplyBaseEntityConfiguration<LogEntity>();
            modelBuilder.ApplyBaseEntityConfiguration<MenuEntity>();

            modelBuilder.ApplyBaseEntityConfiguration<RolePermissionEntity>();
            modelBuilder.ApplyBaseEntityConfiguration<UserRoleEntity>();
            modelBuilder.ApplyBaseEntityConfiguration<RoleMenuEntity>();

            modelBuilder.Entity<MenuEntity>(entity =>
            {
                entity.Property(e => e.Status)
                    .HasDefaultValue(true);
            });
        }
    }

    // Định nghĩa một lớp tĩnh chứa phương thức mở rộng
    public static class ModelBuilderExtensions
    {
        public static void ApplyBaseEntityConfiguration<T>(this ModelBuilder modelBuilder) where T : BaseEntity
        {
            modelBuilder.Entity<T>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.DeletedAt)
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);
            });
        }
    }
}
