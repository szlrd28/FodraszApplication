using FodraszApplication.Data;
using FodraszApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FodraszApplication.Data.Seed
{
    public static class KezdoAdatokSeeder
    {
        public static async Task FuttatasAsync(IServiceProvider sp)
        {
            var db = sp.GetRequiredService<AlkalmazasDbContext>();
            var roleMgr = sp.GetRequiredService<RoleManager<IdentityRole>>();
            var userMgr = sp.GetRequiredService<UserManager<Felhasznalo>>();

            
            await db.Database.MigrateAsync();

           
            async Task EnsureRole(string role)
            {
                if (!await roleMgr.RoleExistsAsync(role))
                    await roleMgr.CreateAsync(new IdentityRole(role));
            }
            await EnsureRole("Fodrasz");
            await EnsureRole("Admin");
            if (!await db.Szolgaltatasok.AnyAsync())
            {
                db.Szolgaltatasok.AddRange(
                    new Szolgaltatas { Kod = "FERFI30", Nev = "Férfi hajvágás", Kategoria = "Alap hajvágások", Perc = 30, ArHuf = 3000, Aktiv = true },
                    new Szolgaltatas { Kod = "NOIROVID30", Nev = "Női hajvágás (rövid)", Kategoria = "Alap hajvágások", Perc = 30, ArHuf = 4000, Aktiv = true },
                    new Szolgaltatas { Kod = "NOIHOSSZU60", Nev = "Női hajvágás (hosszú)", Kategoria = "Alap hajvágások", Perc = 60, ArHuf = 7000, Aktiv = true },
                    new Szolgaltatas { Kod = "GYERMEK30", Nev = "Gyermek hajvágás", Kategoria = "Alap hajvágások", Perc = 30, ArHuf = 2500, Aktiv = true },
                    new Szolgaltatas { Kod = "TOFESTES60", Nev = "Tőfestés", Kategoria = "Festések", Perc = 60, ArHuf = 12000, Aktiv = true },
                    new Szolgaltatas { Kod = "TELJES120", Nev = "Teljes hajfestés", Kategoria = "Festések", Perc = 120, ArHuf = 20000, Aktiv = true },
                    new Szolgaltatas { Kod = "MELIRR120", Nev = "Melír (rövid haj)", Kategoria = "Festések", Perc = 120, ArHuf = 18000, Aktiv = true },
                    new Szolgaltatas { Kod = "MELIRH180", Nev = "Melír (hosszú haj)", Kategoria = "Festések", Perc = 180, ArHuf = 24000, Aktiv = true },
                    new Szolgaltatas { Kod = "OMBRE180", Nev = "Ombre/Balayage", Kategoria = "Festések", Perc = 180, ArHuf = 26000, Aktiv = true },
                    new Szolgaltatas { Kod = "MOSAS30", Nev = "Hajmosás + szárítás", Kategoria = "Egyéb", Perc = 30, ArHuf = 2000, Aktiv = true },
                    new Szolgaltatas { Kod = "ALKALMI60", Nev = "Alkalmi frizura / konty", Kategoria = "Egyéb", Perc = 60, ArHuf = 10000, Aktiv = true },
                    new Szolgaltatas { Kod = "DAUER120", Nev = "Dauer", Kategoria = "Egyéb", Perc = 120, ArHuf = 22000, Aktiv = true },
                    new Szolgaltatas { Kod = "KERATIN60", Nev = "Hajkezelés (keratinos)", Kategoria = "Egyéb", Perc = 60, ArHuf = 15000, Aktiv = true }
                );
                await db.SaveChangesAsync();
            }
            // 3) Felhasználo
            var stylistSeeds = new (string FodraszNev, string Email, string Jelszo)[]
            {
                ("Kata",  "kata@szalon.local",  "Teszt123!"),
                ("Bence", "bence@szalon.local", "Teszt123!"),
                ("Lili",  "lili@szalon.local",  "Teszt123!")
            };
            var adminSeed = (Email: "admin@szalon.local", Jelszo: "Admin123!");

            // Admin
            async Task<Felhasznalo> EnsureUser(string email, string password)
            {
                var u = await userMgr.FindByEmailAsync(email);
                if (u == null)
                {
                    u = new Felhasznalo { UserName = email, Email = email, EmailConfirmed = true };
                    var res = await userMgr.CreateAsync(u, password);
                    if (!res.Succeeded) throw new Exception(string.Join("; ", res.Errors.Select(e => e.Description)));
                }
                return u;
            }

            var adminUser = await EnsureUser(adminSeed.Email, adminSeed.Jelszo);
            if (!await userMgr.IsInRoleAsync(adminUser, "Admin"))
                await userMgr.AddToRoleAsync(adminUser, "Admin");

            
            foreach (var seed in stylistSeeds)
            {
                var user = await EnsureUser(seed.Email, seed.Jelszo);
                if (!await userMgr.IsInRoleAsync(user, "Fodrasz"))
                    await userMgr.AddToRoleAsync(user, "Fodrasz");

                var f = await db.Fodraszok.FirstOrDefaultAsync(x => x.Nev == seed.FodraszNev);
                if (f != null && f.FelhasznaloId != user.Id)
                {
                    f.FelhasznaloId = user.Id;
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
