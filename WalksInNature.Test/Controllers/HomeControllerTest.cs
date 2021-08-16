using MyTested.AspNetCore.Mvc;
using System.Collections.Generic;
using WalksInNature.Controllers;
using Xunit;
using WalksInNature.Services.Walks.Models;
using System;

using static WalksInNature.Test.Data.Walks;
using static WalksInNature.WebConstants.Cache;
using FluentAssertions;

namespace WalksInNature.Test.Controllers
{      
    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnCorrectViewWithModel()
            => MyController<HomeController>
                .Instance(controller => controller
                    .WithData(TenPublicWalks))
                .Calling(x => x.Index())
                .ShouldHave()
                .MemoryCache(cache => cache
                    .ContainingEntry(entry => entry
                        .WithKey(LatestWalksCacheKey)
                        .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromMinutes(15))
                        .WithValueOfType<List<LatestWalkServiceModel>>()))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<List<LatestWalkServiceModel>>());   
                            

        [Fact]
        public void ErrorShouldReturnView()
            => MyController<HomeController>
                .Instance()
                .Calling(x => x.Error())
                .ShouldReturn()
                .View();

        [Fact]
        public void UsefulShouldReturnView()
              => MyController<HomeController>
                 .Instance()
                 .Calling(x => x.Useful())
                 .ShouldReturn()
                 .View();
    }
}
