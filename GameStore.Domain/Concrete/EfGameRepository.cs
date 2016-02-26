using System;
using System.Collections.Generic;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;

namespace GameStore.Domain.Concrete
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
