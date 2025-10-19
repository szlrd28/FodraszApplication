using FodraszApplication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FodraszApplication.Data
{
    public class AlkalmazasDbContext : IdentityDbContext<Felhasznalo>
    {
        public AlkalmazasDbContext(DbContextOptions<AlkalmazasDbContext> options) : base(options) { }

        public DbSet<Fodrasz> Fodraszok => Set<Fodrasz>();
        public DbSet<Idopont> Idopontok => Set<Idopont>();
        public DbSet<FodraszNyitvatartas> Nyitvatartasok => Set<FodraszNyitvatartas>();
        public DbSet<Szolgaltatas> Szolgaltatasok => Set<Szolgaltatas>();
        public DbSet<CegZarvaNap> ZarvaNapok => Set<CegZarvaNap>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);
            b.ApplyConfigurationsFromAssembly(typeof(AlkalmazasDbContext).Assembly);
        }
    }
}
