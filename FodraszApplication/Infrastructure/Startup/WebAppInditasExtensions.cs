using FodraszApplication.Data.Seed;
using Microsoft.Extensions.DependencyInjection;

namespace FodraszApplication.Infrastructure.Startup
{
    public static class WebAppInditasExtensions
    {
        public static async Task MigracioEsSeedFuttatasAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            await KezdoAdatokSeeder.FuttatasAsync(scope.ServiceProvider);
        }
    }
}
