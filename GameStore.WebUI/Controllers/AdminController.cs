using GameStore.WebUI.Models.Abstract;
using GameStore.WebUI.Models.Entities;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.WebUI.Controllers
{
    public class AdminController:Controller
    {
        private IGameRepository _repository;
        public AdminController(IGameRepository repo)
        {
            _repository = repo;
        }
        public ViewResult Index()
        {
            return View(_repository.Games);
        }
        public ViewResult Edit(int id)
        {
            Game game = _repository.Games.FirstOrDefault(m => m.GameId == id);
            return View(game);
        }
    }
}
