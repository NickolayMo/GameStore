using GameStore.WebUI.Models.Entities;
using GameStore.WebUI;
using Microsoft.Data.Entity;

namespace GameStore.WebUI.Models.Concrete
{
    public class EfDbContext:DbContext
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

    }
}
