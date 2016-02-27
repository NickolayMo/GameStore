using GameStore.WebUI.Controllers;
using GameStore.WebUI.Models.Abstract;
using GameStore.WebUI.Models.Entities;
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
            Assert.Equal(result.Count(), 5);
            Assert.Equal(result[0].Name, "Game1");
            Assert.Equal(result[3].Name, "Game4");
        }
    }
}
