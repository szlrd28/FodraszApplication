using System;
using System.ComponentModel.DataAnnotations;

namespace FodraszApplication.Models
{
    public class FodraszNyitvatartas
    {
        public int Id { get; set; }

        [Required] public int FodraszId { get; set; }
        public Fodrasz Fodrasz { get; set; } = default!;

        [Required] public DayOfWeek Nap { get; set; }  

        public bool Zarva { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan Nyitas { get; set; } = TimeSpan.FromHours(8);

        [DataType(DataType.Time)]
        public TimeSpan Zaras { get; set; } = TimeSpan.FromHours(17);
    }
}
