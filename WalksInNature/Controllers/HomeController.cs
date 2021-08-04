﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using WalksInNature.Services.Walks;

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
            const string latestWalksCacheKey = "LatestWalksCacheKey";

            var latestWalks = this.cache.Get<List<LatestWalkServiceModel>>(latestWalksCacheKey);

            if (latestWalks == null)
            {
                latestWalks = this.walkService
                   .Latest()
                   .ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                this.cache.Set(latestWalksCacheKey, latestWalks, cacheOptions);
            }

            return View(latestWalks);

            /*
            var totalStatistics = this.statistics.Total();

            return View(new IndexViewModel 
            {
                TotalWalks = totalStatistics.TotalWalks,
                TotalUsers = totalStatistics.TotalUsers,
                TotalEvents = totalStatistics.TotalEvents,
                Walks = walks
            });*/
        }

        public IActionResult Useful() => View();

        public IActionResult Error() => View();
        
    }
}
