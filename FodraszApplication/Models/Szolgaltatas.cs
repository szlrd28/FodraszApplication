using System.ComponentModel.DataAnnotations;

namespace FodraszApplication.Models
{
    public class Szolgaltatas
    {
        public int Id { get; set; }

        [Required, MaxLength(32)]
        public string Kod { get; set; } = default!; 

        [Required, MaxLength(128)]
        public string Nev { get; set; } = default!; 

        [Required, MaxLength(64)]
        public string Kategoria { get; set; } = "Általános"; 

        [Range(5, 600)]
        public int Perc { get; set; } 

        [Range(0, 1000000)]
        public int ArHuf { get; set; } 

        public bool Aktiv { get; set; } = true; 
    }
}
