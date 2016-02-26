using GameStore.WebUI.Controllers;
using GameStore.WebUI.Models.Abstract;
using GameStore.WebUI.Models.Entities;
using GameStore.WebUI.ModelViews;
using GameStore.WebUI.ViewComponents;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ViewComponents;
using Microsoft.AspNet.Razor.TagHelpers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;


namespace MyFirstDnxUnitTests
{
    public class Class1
    {
        [Fact]
        public void Can_Paginate()
        {
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game { GameId = 1, Name = "Игра1"},
                new Game { GameId = 2, Name = "Игра2"},
                new Game { GameId = 3, Name = "Игра3"},
                new Game { GameId = 4, Name = "Игра4"},
                new Game { GameId = 5, Name = "Игра5"}
            });
            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            List<Game> games = ((GamesListViewModel)controller.List(null, 2).ViewData.Model).Games.ToList();
            Assert.True(games.Count == 2);
            Assert.Equal(games[0].Name, "Игра4");
            Assert.Equal(games[1].Name, "Игра5");

        }
        [Fact]
        public void Can_Filter_Games()
        {
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game { GameId = 1, Category = "Cat1", Name = "Game1"},
                new Game { GameId = 2, Category = "Cat2", Name = "Game2"},
                new Game { GameId = 3, Category = "Cat1", Name = "Game3"},
                new Game { GameId = 4, Category = "Cat2", Name = "Game4"},
                new Game { GameId = 5, Category = "Cat3", Name = "Game5"}
            });
            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;
            List<Game> result = ((GamesListViewModel)controller.List("Cat2", 1).ViewData.Model).Games.ToList();
            Assert.Equal(result.Count(), 2);
            Assert.True(result[0].Name == "Game2" && result[0].Category == "Cat2");
            Assert.True(result[1].Name == "Game4" && result[1].Category == "Cat2");

        }
        [Fact]
        public void Can_Create_Categories()
        {
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                 new Game { GameId = 1, Name = "Игра1", Category="Симулятор"},
                 new Game { GameId = 2, Name = "Игра2", Category="Симулятор"},
                 new Game { GameId = 3, Name = "Игра3", Category="Шутер"},
                 new Game { GameId = 4, Name = "Игра4", Category="RPG"}
            });
            NavViewComponent target = new NavViewComponent(mock.Object);
            ViewViewComponentResult r = (ViewViewComponentResult)target.Invoke();
            List<string> result = ((IEnumerable<string>)r.ViewData.Model).ToList();
            Assert.Equal(result.Count, 3);
            Assert.Equal(result[0], "RPG");
            Assert.Equal(result[1], "Симулятор");
            Assert.Equal(result[2], "Шутер");

        }
        [Fact]
        public void Indicates_Selected_Category()
        {
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new Game[] {
                new Game {GameId = 1, Name="Игра1", Category="Симулятор" },
                new Game {GameId = 2, Name="Игра1", Category="Шутер" }
                
            });
            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;
            string category = "Шутер";        
            NavViewComponent target = new NavViewComponent(mock.Object);
            target.ViewData.Model = (GamesListViewModel)controller.List("Шутер", 1).ViewData.Model;
           ViewViewComponentResult r = (ViewViewComponentResult)target.Invoke();
            string result = (string)target.ViewBag.SelectedCategory;
            Assert.Equal(category, result);

        }
        [Fact]
        public void Generate_Category_Specific_Game_Count()
        {
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(f => f.Games).Returns(new List<Game>
            {
                new Game { GameId = 1, Name="Game1", Category="Category1"},
                new Game { GameId = 2, Name="Game2", Category="Category2"},
                new Game { GameId = 3, Name="Game3", Category="Category1"},
                new Game { GameId = 4, Name="Game4", Category="Category2"},
                new Game { GameId = 5, Name="Game5", Category="Category3"},
            });
            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;
            int res1 = ((GamesListViewModel)controller.List("Category1").ViewData.Model).PagingInfo.TotalItems;
            int res2 = ((GamesListViewModel)controller.List("Category2").ViewData.Model).PagingInfo.TotalItems;
            int res3 = ((GamesListViewModel)controller.List("Category3").ViewData.Model).PagingInfo.TotalItems;
            int resAll = ((GamesListViewModel)controller.List(null).ViewData.Model).PagingInfo.TotalItems;

            Assert.Equal(res1, 2);
            Assert.Equal(res2, 2);
            Assert.Equal(res3, 1);
            Assert.Equal(resAll, 5);
            
                }



    }
}