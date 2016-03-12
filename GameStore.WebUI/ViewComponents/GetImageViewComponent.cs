using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using GameStore.WebUI.Models.Abstract;
using GameStore.WebUI.Models.Entities;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GameStore.WebUI.ViewComponents
{
    public class GetImageViewComponent:ViewComponent
    {
        private IGameRepository _repository;

        public GetImageViewComponent(IGameRepository repo)
        {
            _repository = repo;
        }

        public string Invoke(int? gameId)
        {
            Game game = _repository.Games.FirstOrDefault(g=>
                g.GameId == gameId
            );
           
                FileContentResult result = new FileContentResult(game.ImageData, game.MimeType);
            string content = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(result.FileContents));
            return content;

        }
    }
}
