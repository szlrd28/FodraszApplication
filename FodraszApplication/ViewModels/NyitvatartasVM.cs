/*using System.ComponentModel.DataAnnotations;

namespace FodraszApplication.ViewModels
{
    public class FodraszNyitvatartasVM
    {
        public int FodraszId { get; set; }
        public string FodraszNev { get; set; } = string.Empty;

        public List<NyitvatartasNapVM> Napok { get; set; } = new();
    }

    public class NyitvatartasNapVM
    {
        public DayOfWeek Nap { get; set; }

        public bool Zarva { get; set; }

        [Display(Name = "Nyitás")]
        [RegularExpression(@"^\d{2}:\d{2}$", ErrorMessage = "ÓÓ:PP formátum")]
        public string Nyitas { get; set; } = "08:00";

        [Display(Name = "Zárás")]
        [RegularExpression(@"^\d{2}:\d{2}$", ErrorMessage = "ÓÓ:PP formátum")]
        public string Zaras { get; set; } = "17:00";
    }
}
*/