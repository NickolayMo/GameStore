using GameStore.WebUI.Models.Abstract;
using GameStore.WebUI.ModelViews;
using Microsoft.AspNet.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.WebUI.ViewComponents
{
    // You may need to install the Microsoft.AspNet.Http.Abstractions package into your project
    public class NavViewComponent:ViewComponent
    {
        private IGameRepository _repository;

        public NavViewComponent(IGameRepository repo)
        {
            _repository = repo;
        }

        public IViewComponentResult Invoke()
        {
            GamesListViewModel model = null;
            if (ViewData.Model is GamesListViewModel)
            {
                model = (GamesListViewModel)ViewData.Model;
            }
            
            if(model!= null)
            {
                ViewBag.SelectedCategory = model.CurrentCategory;
            }           
            IEnumerable<string> categories = _repository.Games
                .Select(game => game.Category)
                .Distinct()
                .OrderBy(x => x);
            return View(categories);
        }
    }
       
}
