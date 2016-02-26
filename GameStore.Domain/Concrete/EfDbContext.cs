using GameStore.Domain.Entities;
using Microsoft.Data.

namespace GameStore.Domain.Concrete
{
    public class EfDbContext:DbContext
    {
        public DbSet<Game> Games { get; set; }
    }
}
