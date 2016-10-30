using System.ComponentModel.DataAnnotations;

namespace GameStore.WebUI.ModelViews
{
    public class LoginViewModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}