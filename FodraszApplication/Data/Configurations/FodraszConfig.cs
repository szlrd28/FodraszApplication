using FodraszApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FodraszApplication.Data.Configurations
{
    public class FodraszConfig : IEntityTypeConfiguration<Fodrasz>
    {
        public void Configure(EntityTypeBuilder<Fodrasz> b)
        {
            b.Property(f => f.Nev).IsRequired().HasMaxLength(100);
            b.Property(f => f.FotoUrl).HasMaxLength(256).HasDefaultValue("/img/bildcomming.jpg");

            b.HasOne(f => f.Felhasznalo)
             .WithMany()
             .HasForeignKey(f => f.FelhasznaloId)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasData(
                new Fodrasz { Id = 1, Nev = "Kata", Bemutatkozas = "Női hajvágás, balayage", FotoUrl = "/img/kata.jpg" },
                new Fodrasz { Id = 2, Nev = "Bence", Bemutatkozas = "Férfi vágás, borotválás", FotoUrl = "/img/bence.jpg" },
                new Fodrasz { Id = 3, Nev = "Lili", Bemutatkozas = "Színezés, melír", FotoUrl = "/img/lili.jpg" }
            );
        }
    }
}
