using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WalksInNature.Data;
using WalksInNature.Models.Home;
using WalksInNature.Services.Home;
using WalksInNature.Services.Statistics;

namespace WalksInNature.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStatisticsService statistics;
        private readonly WalksDbContext data;
        private readonly IMapper mapper;

        public HomeController(IStatisticsService statistics, WalksDbContext data, IMapper mapper)
        {
            this.statistics = statistics;
            this.data = data;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            
            var walks = this.data
              .Walks
              .OrderByDescending(x => x.Id)
              .ProjectTo<WalkIndexViewModel>(this.mapper.ConfigurationProvider)            
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

        public IActionResult Useful() => View();

        public IActionResult Error() => View();
        
    }
}
