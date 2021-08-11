using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using WalksInNature.Services.Walks;
using WalksInNature.Services.Walks.Models;

using static WalksInNature.WebConstants.Cache;

namespace WalksInNature.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWalkService walkService;       
        private readonly IMemoryCache cache;       
        public HomeController(IWalkService walkService, IMemoryCache cache)
        {
            this.walkService = walkService;
            this.cache = cache;            
        }

        public IActionResult Index()
        {
            var latestWalks = this.cache.Get<List<LatestWalkServiceModel>>(LatestWalksCacheKey);

            if (latestWalks == null)
            {
                latestWalks = this.walkService
                   .Latest()
                   .ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                this.cache.Set(LatestWalksCacheKey, latestWalks, cacheOptions);
            }

            return View(latestWalks);
            
        }

        public IActionResult Useful() => View();

        public IActionResult Error() => View();
        
    }
}
