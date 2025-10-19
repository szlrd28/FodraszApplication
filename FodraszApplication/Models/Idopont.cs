using System.ComponentModel.DataAnnotations;

namespace FodraszApplication.Models
{
    public class Idopont
    {
        public int Id { get; set; }

        public int FodraszId { get; set; }
        public Fodrasz Fodrasz { get; set; } = default!;   
        public string FelhasznaloId { get; set; } = default!;
        public Felhasznalo Felhasznalo { get; set; } = default!; 
        public DateTime Kezdet { get; set; }             
        public int IdotartamPerc { get; set; }

        public string Szolgaltatas { get; set; } = default!;
        public int? ArHuf { get; set; }

        
        public DateTime Vege => Kezdet.AddMinutes(IdotartamPerc);
    }
}
