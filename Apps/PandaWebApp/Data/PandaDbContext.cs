using Microsoft.EntityFrameworkCore;
using PandaWebApp.Models;

namespace PandaWebApp.Data
{
    public class PandaDbContext : DbContext
    {
        public PandaDbContext()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseSqlServer("Server=.;Database=Panda;Integrated Security=true;");
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Package> Packages { get; set; }

        public DbSet<Receipt> Receipts { get; set; }
    }
}
