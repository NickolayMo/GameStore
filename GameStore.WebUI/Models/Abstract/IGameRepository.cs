using GameStore.WebUI.Models.Entities;
using System.Collections.Generic;

namespace GameStore.WebUI.Models.Abstract
{
    public interface IGameRepository
    {
        IEnumerable<Game> Games { get; }
        void SaveGame(Game game);
        Game DeleteGame(int gameId);
    }
}
