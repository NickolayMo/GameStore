using GameStore.WebUI.Controllers;
using GameStore.WebUI.Models.Abstract;
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
    public class AdminTest
    {
        [Fact]
        public void Index_Contains_All_Games()
        {
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game> {
                new Game {GameId=1, Name="Game1" },
                 new Game {GameId=2, Name="Game2" },
                  new Game {GameId=3, Name="Game3" },
                   new Game {GameId=4, Name="Game4" }
            });
            AdminController controller = new AdminController(mock.Object);
            List<Game> result = ((IEnumerable<Game>)controller.Index().ViewData.Model).ToList();
            Assert.Equal(result.Count(), 4);
            Assert.Equal(result[0].Name, "Game1");
            Assert.Equal(result[3].Name, "Game4");
        }
        [Fact]
        public void Can_Edit_Game()
        {
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game> {
                new Game { GameId = 1, Name="Game1"},
                new Game { GameId = 2, Name="Game2"},
                new Game { GameId = 3, Name="Game3"},
                new Game { GameId = 4, Name="Game4"},
                new Game { GameId = 5, Name="Game5"}               
            });
            AdminController controller = new AdminController(mock.Object);

            Game game1 = controller.Edit(1).ViewData.Model as Game;
            Game game5 = controller.Edit(5).ViewData.Model as Game;

            Assert.Equal(game1.GameId, 1);
            Assert.Equal(game5.GameId, 5);

        }
        [Fact]
        public void Can_Not_Edit_Game()
        {
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game> {
                new Game { GameId = 1, Name="Game1"},
                new Game { GameId = 2, Name="Game2"},
                new Game { GameId = 3, Name="Game3"},
                new Game { GameId = 4, Name="Game4"},
                new Game { GameId = 5, Name="Game5"}
            });
            AdminController controller = new AdminController(mock.Object);

            Game game1 = controller.Edit(6).ViewData.Model as Game;

            Assert.Null(game1);
        
        }
        [Fact]
        public void Can_Save_Valid_Change()
        {
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            AdminController controller = new AdminController(mock.Object);
            Game game = new Game { Name="Test"};
            IActionResult result = controller.Edit(game);
            mock.Verify(m => m.SaveGame(game));
            Assert.IsNotType(typeof(ViewResult), result); 
        }
        [Fact]
        public void Cannot_Save_InValid_Change()
        {
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            AdminController controller = new AdminController(mock.Object);
            Game game = new Game { Name = "Test" };
            controller.ModelState.AddModelError("error", "error");
            IActionResult result = controller.Edit(game);
            mock.Verify(m => m.SaveGame(It.IsAny<Game>()),Times.Never());
            Assert.IsType(typeof(ViewResult), result);
        }
    }
}
