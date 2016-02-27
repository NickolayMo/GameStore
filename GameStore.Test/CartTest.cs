using GameStore.WebUI.Controllers;
using GameStore.WebUI.Models.Entities;
using Microsoft.AspNet.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GameStore.Test
{
    public class CartTest
    {
        [Fact]
        public void Can_Add_New_Lines()
        {
            Game game1 = new Game { GameId = 1, Name = "Game1" };
            Game game2 = new Game { GameId = 2, Name = "Game2" };
            Cart cart = new Cart();

            //action
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            List<CartLine> result = cart.Lines.ToList();
            //asserts
            Assert.Equal(result.Count(), 2);
            Assert.Equal(result[0].Game, game1);
            Assert.Equal(result[1].Game, game2);
        }
        [Fact]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            Game game1 = new Game { GameId = 1, Name = "Game1" };
            Game game2 = new Game { GameId = 2, Name = "Game2" };
            Cart cart = new Cart();
            //actions
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game1,5);
            List<CartLine> result = cart.Lines.OrderBy(c => c.Game.GameId).ToList();

            //asserts
            Assert.Equal(result.Count(), 2);
            Assert.Equal(result[0].Quantity, 6);
            Assert.Equal(result[1].Quantity, 1);

        }
        [Fact]
        public void Can_Remove_Line()
        {
            Game game1 = new Game { GameId = 1, Name = "Game1" };
            Game game2 = new Game { GameId = 2, Name = "Game2" };
            Game game3 = new Game { GameId = 3, Name = "Game3" };
            Cart cart = new Cart();

            cart.AddItem(game1, 1);
            cart.AddItem(game2, 4);
            cart.AddItem(game3, 2);
            cart.AddItem(game2, 1);

            cart.RemoveLine(game2);

            Assert.Equal(cart.Lines.Where(c => c.Game == game2).Count(), 0);
            Assert.Equal(cart.Lines.Count(), 2);
        }
        [Fact]
        public void Calculate_Cart_Total()
        {
            Game game1 = new Game { GameId = 1, Name = "Game1" , Price=100};
            Game game2 = new Game { GameId = 2, Name = "Game2", Price=55 };

            Cart cart = new Cart();

            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game1, 5);
            decimal result = cart.ComputeTotalValue();
            Assert.Equal(result, 655);
        }
        [Fact]
        public void Can_Clear_Content()
        {
            Game game1 = new Game { GameId = 1, Name = "Game1", Price = 100 };
            Game game2 = new Game { GameId = 2, Name = "Game2", Price = 55 };

            Cart cart = new Cart();

            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game1, 5);
            cart.Clear();
            Assert.Equal(cart.Lines.Count(), 0);

        }
        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController controller = new CartController(null, mock.Object);
            ViewResult result = controller.Checkout(cart, shippingDetails);
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());
            Assert.Null(result.ViewName);
            Assert.Equal(false, result.ViewData.ModelState.IsValid);
        }
        [Fact]
        public void Cannot_Checkout_Invalid_ShippongDetails()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Game(), 1);
            CartController controller = new CartController(null, mock.Object);

            controller.ModelState.AddModelError("error", "error");

            ViewResult result = controller.Checkout(cart, new ShippingDetails());
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());
            Assert.Null(result.ViewName);
            Assert.Equal(false, result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Game(), 1);
            CartController controller = new CartController(null, mock.Object);
                       
            ViewResult result = controller.Checkout(cart, new ShippingDetails());
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once());
            Assert.Equal("Completed", result.ViewName);
            Assert.Equal(true, result.ViewData.ModelState.IsValid);
        }
    }
}
