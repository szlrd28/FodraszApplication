using FodraszApplication.Data;
using FodraszApplication.Models;
using FodraszApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FodraszApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminFodraszController : Controller
    {
        private readonly AlkalmazasDbContext _db;
        public AdminFodraszController(AlkalmazasDbContext db) { _db = db; }

        // GET: /AdminFodrasz/Torles/5
        public async Task<IActionResult> Torles(int id)
        {
            var f = await _db.Fodraszok.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (f == null) return NotFound();

            var ossz = await _db.Idopontok.CountAsync(x => x.FodraszId == id);
            var jovo = await _db.Idopontok.CountAsync(x => x.FodraszId == id && x.Kezdet >= DateTime.Today);
            ViewBag.VanFoglalas = ossz > 0;
            ViewBag.Ossz = ossz;
            ViewBag.Jovo = jovo;

            return View(f); 
        }

        // POST: /AdminFodrasz/Torles/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> TorlesMegerositve(int id)
        {
            
            var van = await _db.Idopontok.AnyAsync(x => x.FodraszId == id);
            if (van)
            {
                TempData["Hiba"] = "A fodrász nem törölhető, mert tartoznak hozzá foglalások. Előbb töröld/áthelyezd őket.";
                return RedirectToAction(nameof(Index));
            }

            var f = await _db.Fodraszok.FindAsync(id);
            if (f == null) return NotFound();

            _db.Fodraszok.Remove(f);      
            await _db.SaveChangesAsync();

            TempData["Uzenet"] = "Fodrász törölve.";
            return RedirectToAction(nameof(Index));
        }

        // Lista
        public async Task<IActionResult> Index()
        {
            var lista = await _db.Fodraszok.AsNoTracking().ToListAsync();
            return View(lista);
        }

        // Új fodrász
        public IActionResult Uj() => View(new Fodrasz());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Uj(Fodrasz m)
        {
            if (string.IsNullOrWhiteSpace(m.FotoUrl))
                m.FotoUrl = "/img/bildcomming.jpg";

            if (!ModelState.IsValid) return View(m);

            _db.Fodraszok.Add(m);
            await _db.SaveChangesAsync();
            TempData["Uzenet"] = "Fodrász létrehozva.";
            return RedirectToAction(nameof(Index));
        }
        // GET: /AdminFodrasz/Szerkesztes/5
        [HttpGet]
        public async Task<IActionResult> Szerkesztes(int id)
        {
            var m = await _db.Fodraszok.FindAsync(id);
            if (m == null) return NotFound();
            return View(m);
        }
        [HttpGet]
        public async Task<IActionResult> ZarvaNapok()
        {
            var lista = await _db.ZarvaNapok
                .AsNoTracking()
                .OrderBy(z => z.Datum)
                .ToListAsync();

            return View(lista);
        }

        // Hozzáadás 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ZarvaNapHozzaadas(DateTime datum, string? megjegyzes)
        {
            var day = datum.Date;

            var exists = await _db.ZarvaNapok.AnyAsync(z => z.Datum == day);
            if (exists)
            {
                TempData["Hiba"] = "Erre a napra már van zárva bejegyzés.";
                return RedirectToAction(nameof(ZarvaNapok));
            }

            _db.ZarvaNapok.Add(new CegZarvaNap { Datum = day, Megjegyzes = megjegyzes });
            await _db.SaveChangesAsync();

            TempData["Uzenet"] = "Zárva nap hozzáadva.";
            return RedirectToAction(nameof(ZarvaNapok));
        }

        // Törlés
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ZarvaNapTorles(int id)
        {
            var z = await _db.ZarvaNapok.FindAsync(id);
            if (z == null) return NotFound();

            _db.ZarvaNapok.Remove(z);
            await _db.SaveChangesAsync();

            TempData["Uzenet"] = "Zárva nap törölve.";
            return RedirectToAction(nameof(ZarvaNapok));
        }
        // POST: /AdminFodrasz/Szerkesztes/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Szerkesztes(int id, Fodrasz m)
        {
            if (id != m.Id) return BadRequest();

            if (string.IsNullOrWhiteSpace(m.FotoUrl))
                m.FotoUrl = "/img/bildcomming.jpg";

            if (!ModelState.IsValid) return View(m);

            var existing = await _db.Fodraszok.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null) return NotFound();

            existing.Nev = m.Nev;
            existing.Bemutatkozas = m.Bemutatkozas;
            existing.FotoUrl = m.FotoUrl;

            await _db.SaveChangesAsync();
            TempData["Uzenet"] = "Változtatások mentve.";
            return RedirectToAction(nameof(Index));
        }


       
        public async Task<IActionResult> Nyitvatartas(int id)
        {
            var f = await _db.Fodraszok.FindAsync(id);
            if (f == null) return NotFound();

            var vm = new FodraszNyitvatartasVM { FodraszId = id, FodraszNev = f.Nev };

            var megl = await _db.Nyitvatartasok
                .Where(x => x.FodraszId == id)
                .ToListAsync();

            
            var hu = CultureInfo.GetCultureInfo("hu-HU");
            var sorrend = new[]
            {
        DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
        DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday
    };

            foreach (var nap in sorrend)
            {
                var rec = megl.FirstOrDefault(x => x.Nap == nap);
                vm.Napok.Add(new NyitvatartasNapVM
                {
                    Nap = nap,
                    NapNeve = hu.DateTimeFormat.GetDayName(nap), 
                    Zarva = rec?.Zarva ?? (nap is DayOfWeek.Saturday or DayOfWeek.Sunday),
                    Nyitas = (rec?.Nyitas ?? TimeSpan.FromHours(8)).ToString(@"hh\:mm"),
                    Zaras = (rec?.Zaras ?? TimeSpan.FromHours(17)).ToString(@"hh\:mm")
                });
            }

            return View(vm);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Nyitvatartas(FodraszNyitvatartasVM vm)
        {
            var f = await _db.Fodraszok.FindAsync(vm.FodraszId);
            if (f == null) return NotFound();

            var megl = await _db.Nyitvatartasok
                .Where(x => x.FodraszId == vm.FodraszId)
                .ToListAsync();

            foreach (var n in vm.Napok)
            {
                var rec = megl.FirstOrDefault(x => x.Nap == n.Nap);
                var nyitas = TimeSpan.Parse(n.Nyitas);
                var zaras = TimeSpan.Parse(n.Zaras);

                if (rec == null)
                {
                    rec = new FodraszNyitvatartas
                    {
                        FodraszId = vm.FodraszId,
                        Nap = n.Nap,
                        Zarva = n.Zarva,
                        Nyitas = nyitas,
                        Zaras = zaras
                    };
                    _db.Nyitvatartasok.Add(rec);
                }
                else
                {
                    rec.Zarva = n.Zarva;
                    rec.Nyitas = nyitas;
                    rec.Zaras = zaras;
                }
            }

            await _db.SaveChangesAsync();
            TempData["Uzenet"] = "Nyitvatartás mentve.";
            return RedirectToAction(nameof(Index));
        }
    }
}
