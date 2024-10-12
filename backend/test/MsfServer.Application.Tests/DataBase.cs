using Microsoft.EntityFrameworkCore;
using MsfServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsfServer.Application.Tests
{
    public class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
    {
        public DbSet<TokenEntity> Tokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("TestDatabase");
        }
    }
    public class DataBase
    {
        public static string  connectionString = "Server=NGUYENBACUONG\\CUONGNB;Database=MsfDatabase;Trusted_Connection=True;TrustServerCertificate=true;Pooling=true;Min Pool Size=0;Max Pool Size=100;";
    }
}
