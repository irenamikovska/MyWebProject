using Microsoft.AspNetCore.Builder;
using WalksInNature.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WalksInNature.Data.Models;

namespace WalksInNature.Infrastructure
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();
            
            var data = scopedServices.ServiceProvider.GetService<WalksDbContext>();

            data.Database.Migrate();

            SeedRegions(data);

            SeedLevels(data);

            return app;
        }

        private static void SeedRegions(WalksDbContext data) 
        {
            if (data.Regions.Any())
            {
                return;
            }

            data.Regions.AddRange(new[]
            {
                new Region {Name = "Blagoevgrad"},
                new Region {Name = "Burgas"},
                new Region {Name = "Varna"},
                new Region {Name = "Veliko Tarnovo"},
                new Region {Name = "Vidin"},
                new Region {Name = "Vratsa"},
                new Region {Name = "Gabrovo"},
                new Region {Name = "Dobrich"},
                new Region {Name = "Kardzhali"},
                new Region {Name = "Kyustendil"},
                new Region {Name = "Lovech"},
                new Region {Name = "Montana"},
                new Region {Name = "Pazardzhik"},
                new Region {Name = "Pernik"},
                new Region {Name = "Pleven"},
                new Region {Name = "Plovdiv"},
                new Region {Name = "Razgrad"},
                new Region {Name = "Ruse"},
                new Region {Name = "Silistra"},
                new Region {Name = "Sliven"},
                new Region {Name = "Smolyan"},
                new Region {Name = "Sofia Area"},
                new Region {Name = "Sofia City"},
                new Region {Name = "Stara Zagora"},
                new Region {Name = "Targovishte"},
                new Region {Name = "Haskovo"},
                new Region {Name = "Shumen"},
                new Region {Name = "Yambol"},
            });

            data.SaveChanges();
        }

        private static void SeedLevels(WalksDbContext data)
        {
            if (data.Levels.Any())
            {
                return;
            }

            data.Levels.AddRange(new[]
            { 
                new Level { Name = "Easy"},
                new Level { Name = "Medium"},
                new Level { Name = "Hard"},
            });

            data.SaveChanges();
        }
    }
}
