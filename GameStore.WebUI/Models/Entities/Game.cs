using Microsoft.AspNet.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GameStore.WebUI.Models.Entities
{
    public class Game
    {
        [HiddenInput(DisplayValue =false)]
        public int GameId { get; set; }
        [Required(ErrorMessage ="Пожалуйста введите название игры")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Пожалуйста введите описание игры")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Пожалуйста введите категорию игры")]
        public string Category { get; set; }
        [Required]
        [Range(0.01, double.MaxValue,ErrorMessage = "Введите цену")]
        public decimal Price { get; set; }

        public byte[] ImageData { get; set; }
        public string MimeType { get; set; }

    }
}
