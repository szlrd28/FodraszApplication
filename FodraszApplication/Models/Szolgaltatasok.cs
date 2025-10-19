using System.Collections.Generic;
using System.Linq;

namespace FodraszApplication.Models
{
    public record SzolgaltatasDef(string Kod, string Nev, int Perc, string Kategoria, int ArHuf);

    public static class Szolgaltatasok
    {
        public static readonly List<SzolgaltatasDef> Mind = new()
        {
            // Alap hajvágások
            new("ferfi_vagas",   "Férfi hajvágás",                30, "Alap hajvágások",       4500),
            new("noi_rovid",     "Női hajvágás (rövid haj)",      30, "Alap hajvágások",       5500),
            new("noi_hosszu",    "Női hajvágás (hosszú haj)",     60, "Alap hajvágások",       8500),
            new("gyermek",       "Gyermek hajvágás",              30, "Alap hajvágások",       3500),

            // Festések, színezések
            new("tofestes",      "Tőfestés",                      60, "Festések, színezések",  12000),
            new("teljes_festes", "Teljes hajfestés",             120, "Festések, színezések",  18000),
            new("melir_rovid",   "Melír (rövid haj)",            120, "Festések, színezések",  20000),
            new("melir_hosszu",  "Melír (hosszú haj)",           180, "Festések, színezések",  26000),
            new("ombre_balayage","Ombre/Balayage",               180, "Festések, színezések",  28000),

            // Egyéb szolgáltatások
            new("mosas_szaritas","Hajmosás + szárítás",           30, "Egyéb szolgáltatások",  3000),
            new("alkalmi_frizura","Alkalmi frizura / konty",      60, "Egyéb szolgáltatások",  15000),
            new("dauer",         "Dauer",                        120, "Egyéb szolgáltatások",  22000),
            new("keratinos",     "Hajkezelés (keratinos)",        60, "Egyéb szolgáltatások",  14000),
        };

        public static SzolgaltatasDef Get(string kod) => Mind.First(s => s.Kod == kod);
        public static int Perc(string kod) => Get(kod).Perc;
        public static string Nev(string kod) => Get(kod).Nev;
        public static int Ar(string kod) => Get(kod).ArHuf;
    }
}

