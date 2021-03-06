using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WalksInNature.Data;
using WalksInNature.Data.Models;
using WalksInNature.Infrastructure;
using WalksInNature.Services.Contacts;
using WalksInNature.Services.Events;
using WalksInNature.Services.Guides;
using WalksInNature.Services.Insurances;
using WalksInNature.Services.Levels;
using WalksInNature.Services.Regions;
using WalksInNature.Services.Statistics;
using WalksInNature.Services.Walks;

namespace WalksInNature
{
    public class Startup
    {
        public Startup(IConfiguration configuration) 
            => Configuration = configuration;
        
        public IConfiguration Configuration { get; }
       
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<WalksDbContext>(options => options
                    .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services
                .AddDefaultIdentity<User>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<WalksDbContext>();

            services.AddAutoMapper(typeof(Startup));

            services.AddMemoryCache();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            });

            services.AddTransient<IStatisticsService, StatisticsService>();
            services.AddTransient<IWalkService, WalkService>();
            services.AddTransient<IGuideService, GuideService>();
            services.AddTransient<IEventService, EventService>();
            services.AddTransient<IRegionService, RegionService>();
            services.AddTransient<ILevelService, LevelService>();
            services.AddTransient<IInsuranceService, InsuranceService>();
            services.AddTransient<IContactService, ContactService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.PrepareDatabase();            
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");               
                app.UseHsts();
            }

            app
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "Area",
                        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                    endpoints.MapControllerRoute(
                       name: "Walks Details",
                       pattern: "/Walks/Details/{id}/{information}",
                       defaults: new { controller = "Walks", action = "Details" });

                    endpoints.MapControllerRoute(
                        name: "Event Details",
                        pattern: "/Events/Details/{id}/{information}",
                        defaults: new { controller = "Events", action = "Details"});

                    endpoints.MapDefaultControllerRoute();                   
                    endpoints.MapRazorPages();
                });
        }
    }
}
