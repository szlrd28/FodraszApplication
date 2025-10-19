using System.Collections.Generic;

namespace FodraszApplication.Models
{
    public class Fodrasz
    {
        public int Id { get; set; }
        public string Nev { get; set; } = default!;
        public string? Bemutatkozas { get; set; }
        public string? FotoUrl { get; set; }

        public string? FelhasznaloId { get; set; }
        public Felhasznalo? Felhasznalo { get; set; }

        public ICollection<FodraszNyitvatartas> Nyitvatartasok { get; set; }
            = new List<FodraszNyitvatartas>();

        public ICollection<Idopont> Idopontok { get; set; }
            = new List<Idopont>();
    }
}
