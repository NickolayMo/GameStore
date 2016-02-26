using GameStore.WebUI.Models.Entities;
using System.Collections.Generic;

namespace GameStore.WebUI.ModelViews
{
    public class GamesListViewModel
    {
        public IEnumerable<Game> Games { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}
