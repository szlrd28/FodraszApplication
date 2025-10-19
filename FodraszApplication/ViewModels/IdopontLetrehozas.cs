using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq; // Enumerable.Empty

namespace FodraszApplication.ViewModels
{
    public class IdopontLetrehozasVM
    {
        [Required]
        public int FodraszId { get; set; }
        public IEnumerable<SelectListItem> FodraszLista { get; set; } = Enumerable.Empty<SelectListItem>();

        [Display(Name = "Dátum"), DataType(DataType.Date)]
        public DateTime Datum { get; set; } = DateTime.Today;

        [Required, Display(Name = "Szolgáltatás")]
        public string SzolgaltatasKod { get; set; } = "ferfi_vagas";
        public IEnumerable<SelectListItem> SzolgaltatasLista { get; set; } = Enumerable.Empty<SelectListItem>();

        [Display(Name = "Időtartam (perc)")]
        public int IdotartamPerc { get; set; }

        [Display(Name = "Időpont")]
        public DateTime Kezdet { get; set; }

        
        public IEnumerable<SelectListItem> SzabadIdosavok { get; set; } = Enumerable.Empty<SelectListItem>();

        [Display(Name = "Ár (Ft)")]
        public int ArHuf { get; set; }

        
        public List<string> ZarvaNapok { get; set; } = new();
        public List<string> NyitvaNapok { get; set; } = new();

        
        public bool ZarvaNap { get; set; }
        public string? ZarvaUzenet { get; set; }
        public List<string> CegZarvaNapok { get; set; } = new();
    }
}
