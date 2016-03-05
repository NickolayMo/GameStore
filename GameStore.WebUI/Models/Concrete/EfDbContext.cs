using GameStore.WebUI.Models.Entities;
using GameStore.WebUI;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using GameStore.WebUI.Models.Concrete;

namespace GameStore.WebUI.Models.Concrete
{
    public class EfDbContext: IdentityDbContext<AdminUser>
    {
        public EfDbContext()
        {
            Database.EnsureCreated();
        }
        public DbSet<Game> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            var connectionString = Startup.Configuration["Data:DefaultConnection:ConnectionString"];
            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<AdminUser> AdminUser { get; set; }

    }
}
