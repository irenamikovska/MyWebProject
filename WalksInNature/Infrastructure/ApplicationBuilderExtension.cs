using Microsoft.AspNetCore.Builder;
using WalksInNature.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WalksInNature.Data.Models;
using System;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using static WalksInNature.WebConstants;

namespace WalksInNature.Infrastructure
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            SeedRegions(services);
            SeedLevels(services); 
            SeedAdministrator(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<WalksDbContext>();

            data.Database.Migrate();
        }

        private static void SeedRegions(IServiceProvider services) 
        {
            var data = services.GetRequiredService<WalksDbContext>();

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

        private static void SeedLevels(IServiceProvider services)
        {
            var data = services.GetRequiredService<WalksDbContext>();

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

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task.Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = AdministratorRoleName };

                    await roleManager.CreateAsync(role);

                    const string adminEmail = "admin@walks.com";
                    const string adminPassword = "admin123";

                    var user = new User
                    {
                        Email = adminEmail,
                        UserName = adminEmail,
                        FullName = "Admin"
                    };

                    await userManager.CreateAsync(user, adminPassword);

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
