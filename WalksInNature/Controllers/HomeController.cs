using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using WalksInNature.Data;
using WalksInNature.Models;
using WalksInNature.Models.Home;

namespace WalksInNature.Controllers
{
    public class HomeController : Controller
    {
        private readonly WalksDbContext data;
        public HomeController(WalksDbContext data) => this.data = data;
        public IActionResult Index()
        {
            var totalWalks = this.data.Walks.Count();
            var totalEvents = this.data.Events.Count();
            var totalUsers = this.data.Users.Count();
            
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

            return View(new IndexViewModel 
            {
                TotalWalks = totalWalks,
                TotalUsers = totalUsers,
                TotalEvents = totalEvents,
                Walks = walks
            });
        }     
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        
    }
}
