using FodraszApplication.Data;
using FodraszApplication.Models;
using FodraszApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FodraszApplication.Controllers
{
    [Authorize]
    public class IdopontokController : Controller
    {
        private readonly AlkalmazasDbContext _db;
        private readonly UserManager<Felhasznalo> _userManager;

        public List<string> CegZarvaNapok { get; set; } = new();

        public IdopontokController(AlkalmazasDbContext db, UserManager<Felhasznalo> um)
        {
            _db = db;
            _userManager = um;
        }

       
        private async Task<List<SelectListItem>> BuildSzolgaltatasListaAsync(string? selected = null)
        {
            var all = await _db.Szolgaltatasok
                .AsNoTracking()
                .Where(s => s.Aktiv)
                .OrderBy(s => s.Kategoria).ThenBy(s => s.Nev)
                .ToListAsync();

            var items = new List<SelectListItem>();
            foreach (var grp in all.GroupBy(s => s.Kategoria))
            {
                var g = new SelectListGroup { Name = grp.Key };
                foreach (var s in grp)
                {
                    items.Add(new SelectListItem
                    {
                        Value = s.Kod,
                        Text = $"{s.Nev} ({s.Perc} perc – {s.ArHuf:N0} Ft)",
                        Group = g,
                        Selected = s.Kod == selected
                    });
                }
            }
            return items;
        }

        
        private async Task ToltsdNaptarJeloleseketAsync(IdopontLetrehozasVM vm)
        {
            var start = DateTime.Today;
            var end = start.AddDays(60);

            
            var cegZarva = await _db.ZarvaNapok.AsNoTracking()
    .Where(z => z.Datum >= start && z.Datum <= end)
    .Select(z => z.Datum.Date)
    .ToListAsync();

            var cegZarvaSet = cegZarva.ToHashSet();

            vm.CegZarvaNapok = cegZarva.Select(d => d.ToString("yyyy-MM-dd")).ToList();

            
            var heti = new List<FodraszNyitvatartas>();
            if (vm.FodraszId > 0)
            {
                heti = await _db.Nyitvatartasok.AsNoTracking()
                    .Where(x => x.FodraszId == vm.FodraszId)
                    .ToListAsync();
            }
            var vanHeti = heti.Any();
            var zarvaWeekdays = heti.Where(h => h.Zarva).Select(h => h.Nap).ToHashSet();

            
            bool AlapNyitva(DayOfWeek d) => d is >= DayOfWeek.Monday and <= DayOfWeek.Friday;

            vm.ZarvaNapok.Clear();
            vm.NyitvaNapok.Clear();

            for (var d = start; d <= end; d = d.AddDays(1))
            {
                var date = d.Date;
                bool cegZart = cegZarvaSet.Contains(date);
                bool fodraszZart = vm.FodraszId > 0
                    ? (vanHeti ? zarvaWeekdays.Contains(date.DayOfWeek) : !AlapNyitva(date.DayOfWeek))
                    : false; 

                if (cegZart || fodraszZart)
                    vm.ZarvaNapok.Add(date.ToString("yyyy-MM-dd"));
                else
                    vm.NyitvaNapok.Add(date.ToString("yyyy-MM-dd"));
            }

            
            var nap = vm.Datum.Date;
            if (vm.ZarvaNapok.Contains(nap.ToString("yyyy-MM-dd")))
            {
                vm.ZarvaNap = true;
                vm.ZarvaUzenet = "Ezen a napon nem foglalható időpont (zárva).";
            }
            else
            {
                vm.ZarvaNap = false;
                vm.ZarvaUzenet = null;
            }
        }

       
        private async Task FeltoltAsync(IdopontLetrehozasVM vm)
        {
           
            vm.FodraszLista = await _db.Fodraszok.AsNoTracking()
                .Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Nev })
                .ToListAsync();

            
            vm.SzolgaltatasLista = await BuildSzolgaltatasListaAsync(vm.SzolgaltatasKod);
            var sz = await _db.Szolgaltatasok.AsNoTracking()
                .Where(x => x.Aktiv)
                .OrderBy(x => x.Kategoria).ThenBy(x => x.Nev)
                .FirstOrDefaultAsync(x => x.Kod == vm.SzolgaltatasKod);

            if (sz == null)
            {
                sz = await _db.Szolgaltatasok.AsNoTracking()
                    .Where(x => x.Aktiv)
                    .OrderBy(x => x.Kategoria).ThenBy(x => x.Nev)
                    .FirstOrDefaultAsync();
                vm.SzolgaltatasKod = sz?.Kod ?? vm.SzolgaltatasKod;
            }

            vm.IdotartamPerc = sz?.Perc ?? 60;
            vm.ArHuf = sz?.ArHuf ?? 0;

        
            await ToltsdNaptarJeloleseketAsync(vm);

           
            if (vm.FodraszId <= 0 || vm.ZarvaNap)
            {
                vm.SzabadIdosavok = Enumerable.Empty<SelectListItem>();
                return;
            }

            var nap = vm.Datum.Date;

            // alap 08–17
            DateTime munkaStart = nap.AddHours(8);
            DateTime munkaVege = nap.AddHours(17);

            // egyedi nyitvatartás a napra
            var nyit = await _db.Nyitvatartasok.AsNoTracking()
                .FirstOrDefaultAsync(x => x.FodraszId == vm.FodraszId && x.Nap == vm.Datum.DayOfWeek);

            if (nyit != null)
            {
                if (nyit.Zarva)
                {
                    vm.ZarvaNap = true;
                    vm.ZarvaUzenet = "A kiválasztott fodrász ezen a napon nem dolgozik.";
                    vm.SzabadIdosavok = Enumerable.Empty<SelectListItem>();
                    return;
                }
                munkaStart = nap.Add(nyit.Nyitas);
                munkaVege = nap.Add(nyit.Zaras);
            }

            // ha túl rövid a nap a választott szolgáltatáshoz
            if (munkaVege <= munkaStart || munkaStart.AddMinutes(vm.IdotartamPerc) > munkaVege)
            {
                vm.SzabadIdosavok = Enumerable.Empty<SelectListItem>();
                vm.ZarvaNap = true;
                vm.ZarvaUzenet = "Ezen a napon nincs elég idő a választott szolgáltatáshoz.";
                return;
            }

            var from = nap;
            var to = nap.AddDays(1);

            // foglalt idősávok az adott fodrásznál, ezen a napon
            var foglalt = await _db.Idopontok.AsNoTracking()
                .Where(i => i.FodraszId == vm.FodraszId && i.Kezdet >= from && i.Kezdet < to)
                .ToListAsync();

            // szabad idősávok
            var items = new List<SelectListItem>();
            var lepes = vm.IdotartamPerc;

            for (var t = munkaStart; t <= munkaVege.AddMinutes(-vm.IdotartamPerc); t = t.AddMinutes(lepes))
            {
                var end = t.AddMinutes(vm.IdotartamPerc);
                var utkozik = foglalt.Any(i => i.Kezdet < end && t < i.Kezdet.AddMinutes(i.IdotartamPerc));
                if (!utkozik)
                {
                    items.Add(new SelectListItem
                    {
                        Value = t.ToString("yyyy-MM-ddTHH:mm:ss"),
                        Text = t.ToString("HH:mm")
                    });
                }
            }

            vm.SzabadIdosavok = items;
            if (items.Any())
            {
                vm.Kezdet = DateTime.Parse(items.First().Value);
            }
            else
            {
                vm.ZarvaNap = true;
                vm.ZarvaUzenet = "Nincs szabad időpont a megadott beállításokkal.";
            }
        }

        // GET: Időpont létrehozás
        public async Task<IActionResult> Letrehozas(int? fodraszId, DateTime? datum, string? szolgaltatasKod)
        {
            if (string.IsNullOrWhiteSpace(szolgaltatasKod))
            {
                szolgaltatasKod = await _db.Szolgaltatasok.AsNoTracking()
                    .Where(s => s.Aktiv)
                    .OrderBy(s => s.Kategoria).ThenBy(s => s.Nev)
                    .Select(s => s.Kod)
                    .FirstOrDefaultAsync();
            }

            var vm = new IdopontLetrehozasVM
            {
                FodraszId = fodraszId ?? 0,
                Datum = (datum ?? DateTime.Today).Date,
                SzolgaltatasKod = szolgaltatasKod ?? "ferfi_vagas"
            };

            await FeltoltAsync(vm);
            return View(vm);
        }

        //POST: Foglalás mentése
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Letrehozas(IdopontLetrehozasVM vm)
        {
            var sz = await _db.Szolgaltatasok.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Kod == vm.SzolgaltatasKod && x.Aktiv);

            if (sz == null)
            {
                ModelState.AddModelError(nameof(IdopontLetrehozasVM.SzolgaltatasKod), "Érvénytelen szolgáltatás.");
            }
            else
            {
                vm.IdotartamPerc = sz.Perc;
                vm.ArHuf = sz.ArHuf;
            }

            
            var nap = vm.Datum.Date;
            var cegZart = await _db.ZarvaNapok.AnyAsync(z => z.Datum == nap);
            var nyit = await _db.Nyitvatartasok
                .FirstOrDefaultAsync(x => x.FodraszId == vm.FodraszId && x.Nap == vm.Datum.DayOfWeek);
            if (cegZart)
                ModelState.AddModelError(nameof(IdopontLetrehozasVM.Datum), "Ezen a napon a szalon zárva tart.");
            if (nyit != null && nyit.Zarva)
                ModelState.AddModelError(nameof(IdopontLetrehozasVM.Datum), "A kiválasztott fodrász ezen a napon nem dolgozik.");

            if (!ModelState.IsValid)
            {
                await FeltoltAsync(vm);
                return View(vm);
            }

            var userId = _userManager.GetUserId(User)!;
            var kezdet = DateTime.SpecifyKind(vm.Kezdet, DateTimeKind.Local);
            var vege = kezdet.AddMinutes(vm.IdotartamPerc);

            
            var utkozik = await _db.Idopontok.AnyAsync(i =>
                i.FodraszId == vm.FodraszId &&
                i.Kezdet < vege &&
                kezdet < i.Kezdet.AddMinutes(i.IdotartamPerc));

            if (utkozik)
            {
                ModelState.AddModelError(nameof(IdopontLetrehozasVM.Kezdet),
                    "Közben lefoglalták ezt az időpontot. Válassz másikat.");
                await FeltoltAsync(vm);
                return View(vm);
            }

            _db.Idopontok.Add(new Idopont
            {
                FodraszId = vm.FodraszId,
                FelhasznaloId = userId,
                Kezdet = kezdet,
                IdotartamPerc = vm.IdotartamPerc,
                Szolgaltatas = sz!.Nev,
                ArHuf = sz.ArHuf
            });
            await _db.SaveChangesAsync();

            TempData["Uzenet"] = "Sikeres foglalás!";
            TempData["UtolsoFodraszId"] = vm.FodraszId;
            return RedirectToAction(nameof(Enyem));
        }

        //Saját foglalások
        public async Task<IActionResult> Enyem()
        {
            var userId = _userManager.GetUserId(User)!;

            var listaDb = await _db.Idopontok
                .Include(a => a.Fodrasz)
                .Where(a => a.FelhasznaloId == userId)
                .OrderBy(a => a.Kezdet)
                .ToListAsync();

            
            var arIndex = await _db.Szolgaltatasok.AsNoTracking()
                .ToDictionaryAsync(s => s.Nev, s => s.ArHuf);

            var lista = listaDb.Select(i => new UgyfelFoglalasSorVM
            {
                Id = i.Id,
                FodraszNev = i.Fodrasz.Nev,
                Kezdet = i.Kezdet,
                IdotartamPerc = i.IdotartamPerc,
                Szolgaltatas = i.Szolgaltatas,
                ArHuf = i.ArHuf ?? (arIndex.TryGetValue(i.Szolgaltatas, out var ar) ? ar : 0)
            }).ToList();

            return View(lista);
        }

        //Lemondás
        [HttpPost("/Idopontok/Lemondas")]
        [ValidateAntiForgeryToken]
        [ActionName("Lemondas")]
        public async Task<IActionResult> LemondasPost(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            var appt = await _db.Idopontok
                .FirstOrDefaultAsync(x => x.Id == id && x.FelhasznaloId == userId);

            if (appt == null) return NotFound();

            _db.Idopontok.Remove(appt);
            await _db.SaveChangesAsync();

            TempData["Uzenet"] = "Foglalás lemondva.";
            return RedirectToAction(nameof(Enyem));
        }
    }
}
