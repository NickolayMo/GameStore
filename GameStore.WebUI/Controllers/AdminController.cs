using GameStore.WebUI.Models.Abstract;
using GameStore.WebUI.Models.Entities;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.WebUI.Controllers
{
    [Authorize]
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
        public ViewResult Edit(int? id)
        {
            Game game = _repository.Games.FirstOrDefault(m => m.GameId == id);
            return View(game);
        }
        [HttpPost]
        public IActionResult Edit(Game game, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                   
                    using (var reader = new BinaryReader(image.OpenReadStream()))
                    {
                        game.ImageData = new byte[image.Length];
                        game.MimeType = image.ContentType;
                        int index = 0;
                        int count = Convert.ToInt32(image.Length);                        
                        reader.Read(game.ImageData, index, count);

                    }
                    
                }
                _repository.SaveGame(game);
                if (TempData!= null)
                {
                    TempData["message"] = $"Изменения в игре {game.Name} были сохранены";
                }                
                return RedirectToAction("Index");
            }
            return View(game);
        }
        public ViewResult Create()
        {
            return View("Edit", new Game());
        }
        [HttpPost]
        public IActionResult Delete(int gameId)
        {
            Game deletedGame = _repository.DeleteGame(gameId);
            if (deletedGame != null)
            {
                TempData["message"] = $"Игра {deletedGame.Name} была удалена";
            }
            return RedirectToAction("Index");
        }
        
    }
}
