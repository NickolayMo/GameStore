using GameStore.WebUI.Models.Abstract;
using GameStore.WebUI.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Data.Entity;

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

        public Game DeleteGame(int gameId)
        {
            Game game = _context.Games.Single(g => g.GameId == gameId);
            if (game != null)
            {
                _context.Remove(game);
                _context.SaveChanges();
            }
            return game;


        }

        public void SaveGame(Game game)
        {
            if (game.GameId == 0)
            {
                _context.Games.Add(game);
            }
            else
            {
                _context.Update(game);
               
            }
            _context.SaveChanges();
        }
    }
}
