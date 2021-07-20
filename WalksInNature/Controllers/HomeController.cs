using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using WalksInNature.Data;
using WalksInNature.Models;
using WalksInNature.Models.Home;
using WalksInNature.Services.Statistics;

namespace WalksInNature.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStatisticsService statistics;
        private readonly WalksDbContext data;
        public HomeController(IStatisticsService statistics, WalksDbContext data)
        {
            this.statistics = statistics;
            this.data = data;
        }
         
        public IActionResult Index()
        {
                       
            var walks = this.data
                    .Walks
                    .OrderByDescending(x => x.Id)
                    .Select(x => new WalkIndexViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ImageUrl = x.ImageUrl,
                        Region = x.Region.Name                        
                    })
                    .Take(3)
                    .ToList();

            var totalStatistics = this.statistics.Total();

            return View(new IndexViewModel 
            {
                TotalWalks = totalStatistics.TotalWalks,
                TotalUsers = totalStatistics.TotalUsers,
                TotalEvents = totalStatistics.TotalEvents,
                Walks = walks
            });
        }     
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        
    }
}
