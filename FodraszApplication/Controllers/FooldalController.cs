using FodraszApplication.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FodraszApplication.Controllers
{
    public class FooldalController : Controller
    {
        private readonly AlkalmazasDbContext _db;
        public FooldalController(AlkalmazasDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Fodrasz") || User.IsInRole("Admin"))
                return RedirectToAction("Foglalasaim", "FodraszPanel");

            var stylists = await _db.Fodraszok.AsNoTracking().ToListAsync();
            return View(stylists);
        }
    }
}
