using FodraszApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FodraszApplication.Data.Configurations
{
    public class CegZarvaNapConfig : IEntityTypeConfiguration<CegZarvaNap>
    {
        public void Configure(EntityTypeBuilder<CegZarvaNap> b)
        {
            b.Property(x => x.Datum).HasColumnType("date");
            b.HasIndex(x => x.Datum).IsUnique();
        }
    }
}
