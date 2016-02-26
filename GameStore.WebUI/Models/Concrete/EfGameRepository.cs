using GameStore.WebUI.Models.Abstract;
using GameStore.WebUI.Models.Entities;
using System.Collections.Generic;
namespace GameStore.WebUI.Models.Concrete
{
    public class EfGameRepository : IGameRepository
    {
        private EfDbContext _context = new EfDbContext();
        public IEnumerable<Game> Games
        {
            get
            {
                return _context.Games;

            }
        }
    }
}
