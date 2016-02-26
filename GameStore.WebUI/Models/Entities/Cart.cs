using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.WebUI.Models.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();
        public void AddItem(Game game, int quantity)
        {
            CartLine line = lineCollection
                .Where(g => g.Game.GameId == game.GameId)
                .FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartLine { Game = game, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public void RemoveLine(Game game)
        {
            lineCollection.RemoveAll(g => g.Game.GameId == game.GameId);
        }

        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(g => g.Game.Price * g.Quantity);
        }
        public void Clear()
        {
            lineCollection.Clear();
        }
        public IEnumerable<CartLine> Lines
        {
            get
            {
                return lineCollection;
            }

        }

    }

    public class CartLine
    {
        public Game Game { get; set; }
        public int Quantity { get; set; }
    }
}
