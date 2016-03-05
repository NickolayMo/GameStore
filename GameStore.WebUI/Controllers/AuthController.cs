using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using GameStore.WebUI.Models.Concrete;
using GameStore.WebUI.ModelViews;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace GameStore.WebUI.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<AdminUser> _signInManager;
        public AuthController(SignInManager<AdminUser> manager)
        {
            _signInManager = manager;
        }
        public IActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (string.IsNullOrWhiteSpace(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(model.Login, model.Password, true, false);
                if (string.IsNullOrWhiteSpace(returnUrl))
                {
                    return RedirectToAction("Index", "Admin");
                }
                return Redirect(returnUrl);
            }
            ModelState.AddModelError("", "Логин или праль неверный");
            return View();

        }
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }
            return RedirectToRoute("Start");
        }
        
    }
}
