using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FodraszApplication.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
       
        [HttpGet("/after-login")]
        public IActionResult AfterLogin()
        {
            if (User.IsInRole("Fodrasz") || User.IsInRole("Admin"))
                return RedirectToAction("Foglalasaim", "FodraszPanel");

            
            return RedirectToAction("Index", "Fooldal");
        }
    }
}
