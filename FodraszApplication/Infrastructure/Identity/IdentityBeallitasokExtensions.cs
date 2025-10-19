using System.IO;
using FodraszApplication.Data;
using FodraszApplication.Models;
using FodraszApplication.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FodraszApplication.Infrastructure.Identity
{
    public static class IdentityBeallitasokExtensions
    {
        public static IServiceCollection AddAlkalmazasIdentity(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment env)
        {
            
            var keysPath = Path.Combine(env.ContentRootPath, "dpkeys");
            Directory.CreateDirectory(keysPath);
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(keysPath))
                .SetApplicationName("FodraszApplication");

            
            services.AddScoped<IUserClaimsPrincipalFactory<Felhasznalo>, FelhasznaloClaimsFactory>();

            
            services.AddDefaultIdentity<Felhasznalo>(opt =>
            {
                opt.SignIn.RequireConfirmedAccount = false;
                opt.Password.RequiredLength = 6;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AlkalmazasDbContext>();

            
            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.Name = "FodraszAuth";
                opt.ExpireTimeSpan = TimeSpan.FromDays(30);
                opt.SlidingExpiration = true;
                opt.LoginPath = "/Identity/Account/Login";
                opt.LogoutPath = "/Identity/Account/Logout";
                opt.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });

            return services;
        }
    }
}
