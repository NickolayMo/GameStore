﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using GameStore.WebUI.Models.Abstract;
using GameStore.WebUI.Models.Entities;
using Microsoft.AspNet.Http;
using Newtonsoft.Json;
using GameStore.WebUI.ModelViews;
using GameStore.WebUI.Ifrastructure.Binders;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GameStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IGameRepository _repository;
        private IOrderProcessor _processor;

        public CartController(IGameRepository repo, IOrderProcessor processor)
        {
            _repository = repo;
            _processor = processor;

        }
        //cart получаем из CartModelBinder
        public RedirectToActionResult AddToCart(Cart cart, int gameId, string returnUrl)
        {
            Game game = _repository.Games.FirstOrDefault(g => g.GameId == gameId);
            if (game != null)
            {
                cart.AddItem(game, 1);
                CartModelBinder.SetUpCart(cart,HttpContext.Session);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public RedirectToActionResult RemoveFromCart(Cart cart, int gameId, string returnUrl)
        {
            Game game = _repository.Games.FirstOrDefault(g => g.GameId == gameId);
            if(game != null)
            {               
                cart.RemoveLine(game);
                if (HttpContext != null)
                {
                    CartModelBinder.SetUpCart(cart, HttpContext.Session);
                }                

            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl

            });

        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count()==0)
            {
                ModelState.AddModelError("", "Извените ваша корзина пуста");

            }
            if (ModelState.IsValid)
            {                
                _processor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                if (HttpContext != null)
                {
                    CartModelBinder.SetUpCart(cart, HttpContext.Session);
                }                
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
            
        }

    }
}
