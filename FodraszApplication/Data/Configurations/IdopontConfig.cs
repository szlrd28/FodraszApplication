using FodraszApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FodraszApplication.Data.Configurations
{
    public class IdopontConfig : IEntityTypeConfiguration<Idopont>
    {
        public void Configure(EntityTypeBuilder<Idopont> b)
        {
            b.HasIndex(x => new { x.FodraszId, x.Kezdet });

            b.HasOne(i => i.Fodrasz)
             .WithMany(f => f.Idopontok)
             .HasForeignKey(i => i.FodraszId)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(i => i.Felhasznalo)
             .WithMany()
             .HasForeignKey(i => i.FelhasznaloId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
