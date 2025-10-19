using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using FodraszApplication.Data;
using FodraszApplication.Models;

namespace FodraszApplication.Services
{
    public class FelhasznaloClaimsFactory : UserClaimsPrincipalFactory<Felhasznalo, IdentityRole>
    {
        private readonly AlkalmazasDbContext _db;

        public FelhasznaloClaimsFactory(
            UserManager<Felhasznalo> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor,
            AlkalmazasDbContext db) : base(userManager, roleManager, optionsAccessor)
        {
            _db = db;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(Felhasznalo user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            var fodrasz = await _db.Fodraszok.AsNoTracking()
                .FirstOrDefaultAsync(f => f.FelhasznaloId == user.Id);

            if (fodrasz != null)
            {
                identity.AddClaim(new Claim("FodraszId", fodrasz.Id.ToString()));
                identity.AddClaim(new Claim("FodraszNev", fodrasz.Nev));
            }
            return identity;
        }
    }
}
