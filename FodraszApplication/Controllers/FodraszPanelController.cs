using FodraszApplication.Data;
using FodraszApplication.Models;
using FodraszApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FodraszApplication.Controllers
{
    [Authorize(Roles = "Fodrasz,Admin")]
    public class FodraszPanelController : Controller
    {
        private readonly AlkalmazasDbContext _db;
        private readonly UserManager<Felhasznalo> _um;

        public FodraszPanelController(AlkalmazasDbContext db, UserManager<Felhasznalo> um)
        {
            _db = db;
            _um = um;
        }

        
        //public async Task<IActionResult> Foglalasaim(DateTime? tol = null, DateTime? ig = null)
        public async Task<IActionResult> Foglalasaim(DateTime? tol = null, DateTime? ig = null)
        {
            var userId = _um.GetUserId(User)!;
            var fodrasz = await _db.Fodraszok.AsNoTracking()
                .FirstOrDefaultAsync(f => f.FelhasznaloId == userId);

            if (fodrasz == null && !User.IsInRole("Admin"))
                return Forbid();

            var from = (tol ?? DateTime.Today).Date;
            var to = (ig ?? DateTime.Today.AddDays(30)).Date.AddDays(1);

            var q = _db.Idopontok
                .Include(i => i.Felhasznalo)
                .Include(i => i.Fodrasz)
                .AsNoTracking()
                .Where(i => i.Kezdet >= from && i.Kezdet < to);

            if (!User.IsInRole("Admin"))
                q = q.Where(i => i.FodraszId == fodrasz!.Id);

            var lista = await q.OrderBy(i => i.Kezdet)
                .Select(i => new FodraszApplication.ViewModels.FodraszFoglalasSorVM
                {
                    IdopontId = i.Id,
                    Kezdet = i.Kezdet,
                    Vege = i.Kezdet.AddMinutes(i.IdotartamPerc),
                    UgyfelEmail = i.Felhasznalo.Email!,
                    UgyfelNev = i.Felhasznalo.UserName,
                    Szolgaltatas = i.Szolgaltatas,
                    IdotartamPerc = i.IdotartamPerc,
                    FodraszNev = i.Fodrasz.Nev,   
                    ArHuf = i.ArHuf ?? 0
                })
                .ToListAsync();

            ViewBag.FodraszNev = fodrasz?.Nev ?? (User.IsInRole("Admin") ? "Összes fodrász" : "Ismeretlen");
            ViewBag.Tol = from.Date;
            ViewBag.Ig = to.AddDays(-1).Date;

            return View(lista);  
        }

    }
}
