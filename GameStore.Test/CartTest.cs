using GameStore.WebUI.Models.Entities;
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
    }
}
