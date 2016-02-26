using System.Linq;
using GameStore.WebUI.Models.Abstract;
using Microsoft.AspNet.Mvc;
using GameStore.WebUI.ModelViews;
using Moq;
using System.Collections.Generic;
using GameStore.WebUI.Models.Entities;
using GameStore.WebUI.ViewComponents;
using Microsoft.AspNet.Mvc.ViewComponents;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GameStore.WebUI.Controllers
{
    public class GameController : Controller
    {
        private IGameRepository _repository;
        public int pageSize = 4;

        public GameController(IGameRepository repo)
        {
            _repository = repo;

        }
        // GET: /<controller>/
        public ViewResult List(string category, int page = 1)
        {
            GamesListViewModel model = new GamesListViewModel
            {
                Games = _repository.Games
                .Where(p => category == null || p.Category == category)
                     .OrderBy(game => game.GameId)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = (category == null) ? _repository.Games.Count() :
                    _repository.Games.Where(s=>s.Category == category).Count()
                },
                CurrentCategory = category

            };
            return View(model);
        }

    }
}
