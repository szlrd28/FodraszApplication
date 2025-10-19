namespace FodraszApplication.ViewModels
{
    public class FodraszFoglalasSorVM
    {
        public int IdopontId { get; set; }
        public DateTime Kezdet { get; set; }
        public DateTime Vege { get; set; }
        public string UgyfelEmail { get; set; } = default!;
        public string? UgyfelNev { get; set; }
        public string Szolgaltatas { get; set; } = default!;
        public int IdotartamPerc { get; set; }
        public int ArHuf { get; set; }
        public string FodraszNev { get; set; } = default!;
    }
}
