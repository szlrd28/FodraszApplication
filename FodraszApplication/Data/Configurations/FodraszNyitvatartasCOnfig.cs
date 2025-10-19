using FodraszApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FodraszApplication.Data.Configurations
{
    public class FodraszNyitvatartasConfig : IEntityTypeConfiguration<FodraszNyitvatartas>
    {
        public void Configure(EntityTypeBuilder<FodraszNyitvatartas> b)
        {
            b.HasOne(n => n.Fodrasz)
             .WithMany(f => f.Nyitvatartasok)
             .HasForeignKey(n => n.FodraszId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasIndex(x => new { x.FodraszId, x.Nap }).IsUnique();
        }
    }
}
