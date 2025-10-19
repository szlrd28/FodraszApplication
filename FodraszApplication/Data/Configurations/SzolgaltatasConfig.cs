using FodraszApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FodraszApplication.Data.Configurations
{
    public class SzolgaltatasConfig : IEntityTypeConfiguration<Szolgaltatas>
    {
        public void Configure(EntityTypeBuilder<Szolgaltatas> b)
        {
            b.HasIndex(s => s.Kod).IsUnique();
            b.Property(s => s.Nev).IsRequired().HasMaxLength(120);
            b.Property(s => s.Kategoria).IsRequired().HasMaxLength(80);
        }
    }
}
