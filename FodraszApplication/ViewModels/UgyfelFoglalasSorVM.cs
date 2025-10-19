namespace FodraszApplication.ViewModels
{
    public class UgyfelFoglalasSorVM
    {
        public int Id { get; set; }
        public string FodraszNev { get; set; } = default!;
        public DateTime Kezdet { get; set; }
        public DateTime Vege => Kezdet.AddMinutes(IdotartamPerc);
        public int IdotartamPerc { get; set; }
        public string Szolgaltatas { get; set; } = default!;
        public int ArHuf { get; set; }
    }
}

