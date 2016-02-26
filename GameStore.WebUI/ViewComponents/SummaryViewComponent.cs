using GameStore.WebUI.Ifrastructure.Binders;
using GameStore.WebUI.Models.Abstract;
using GameStore.WebUI.Models.Entities;
using Microsoft.AspNet.Mvc;

namespace GameStore.WebUI.ViewComponents
{
    public class SummaryViewComponent : ViewComponent
    {
        private IGameRepository _repository;

        public SummaryViewComponent(IGameRepository repo)
        {
            _repository = repo;
        }

        public IViewComponentResult Invoke()
        {
            Cart cart = CartModelBinder.GetCart(HttpContext.Session);
            return View(cart);
        }
    }
}
