using FodraszApplication.Data;
using FodraszApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FodraszApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("AdminSzolgaltatas")]
    public class AdminSzolgaltatasController : Controller
    {
        private readonly AlkalmazasDbContext _db;
        public AdminSzolgaltatasController(AlkalmazasDbContext db) { _db = db; }

        // GET: /AdminSzolgaltatas
        [HttpGet("")]
        public async Task<IActionResult> Index(string? q = null)
        {
            var query = _db.Szolgaltatasok.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(s => s.Nev.Contains(q) || s.Kod.Contains(q) || s.Kategoria.Contains(q));

            var lista = await query.OrderBy(s => s.Kategoria).ThenBy(s => s.Nev).ToListAsync();
            return View(lista);
        }

        // GET: /AdminSzolgaltatas/Uj
        [HttpGet("Uj")]
        public IActionResult Uj() => View(new Szolgaltatas { Aktiv = true });

        // POST: /AdminSzolgaltatas/Uj
        [HttpPost("Uj"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Uj(Szolgaltatas m)
        {
            if (await _db.Szolgaltatasok.AnyAsync(s => s.Kod == m.Kod))
                ModelState.AddModelError(nameof(Szolgaltatas.Kod), "Ez a kód már létezik.");

            if (!ModelState.IsValid) return View(m);

            _db.Szolgaltatasok.Add(m);
            await _db.SaveChangesAsync();
            TempData["Uzenet"] = "Szolgáltatás létrehozva.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /AdminSzolgaltatas/Szerkesztes/5
        [HttpGet("Szerkesztes/{id:int}")]
        public async Task<IActionResult> Szerkesztes(int id)
        {
            var m = await _db.Szolgaltatasok.FindAsync(id);
            return m == null ? NotFound() : View(m);
        }

        // POST: /AdminSzolgaltatas/Szerkesztes/5
        [HttpPost("Szerkesztes/{id:int}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Szerkesztes(int id, Szolgaltatas m)
        {
            if (id != m.Id) return BadRequest();

            if (await _db.Szolgaltatasok.AnyAsync(s => s.Kod == m.Kod && s.Id != m.Id))
                ModelState.AddModelError(nameof(Szolgaltatas.Kod), "Ez a kód már létezik.");

            if (!ModelState.IsValid) return View(m);

            _db.Entry(m).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            TempData["Uzenet"] = "Módosítások mentve.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /AdminSzolgaltatas/Archiv/5
        [HttpPost("Archiv/{id:int}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Archiv(int id)
        {
            var m = await _db.Szolgaltatasok.FindAsync(id);
            if (m == null) return NotFound();
            m.Aktiv = !m.Aktiv;
            await _db.SaveChangesAsync();
            TempData["Uzenet"] = m.Aktiv ? "Szolgáltatás újra aktív." : "Szolgáltatás archiválva.";
            return RedirectToAction(nameof(Index));
        }
    }
}
