using FodraszApplication.Data;
using FodraszApplication.Infrastructure.Identity;
using FodraszApplication.Infrastructure.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddDbContext<AlkalmazasDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
       
       .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
       .ConfigureWarnings(w => w.Log(RelationalEventId.PendingModelChangesWarning))
);


builder.Services.AddAlkalmazasIdentity(builder.Configuration, builder.Environment);


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Hiba");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "adminFodraszShort",
    pattern: "AdminFodrasz",
    defaults: new { controller = "AdminFodrasz", action = "Index" }
);


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Fooldal}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "home-short",
    pattern: "Home",
    defaults: new { controller = "Fooldal", action = "Index" });

app.MapRazorPages();


await app.MigracioEsSeedFuttatasAsync();

app.Run();
