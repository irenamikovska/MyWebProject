using MyTested.AspNetCore.Mvc;
using System.Collections.Generic;
using WalksInNature.Controllers;
using WalksInNature.Services.Walks.Models;
using Xunit;

using static WalksInNature.Test.Data.Walks;

namespace WalksInNature.Test.Pipeline
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnViewWithCorrectModelAndData()
            => MyMvc
                .Pipeline()
                .ShouldMap("/")
                .To<HomeController>(x => x.Index())
                .Which(controller => controller
                    .WithData(TenPublicWalks))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<List<LatestWalkServiceModel>>());                   

        [Fact]
        public void ErrorShouldReturnView()
            => MyMvc
                .Pipeline()
                .ShouldMap("/Home/Error")
                .To<HomeController>(x => x.Error())
                .Which()
                .ShouldReturn()
                .View();

        [Fact]
        public void UsefulShouldReturnView()
          => MyMvc
              .Pipeline()
              .ShouldMap("/Home/Useful")
              .To<HomeController>(x => x.Useful())
              .Which()
              .ShouldReturn()
              .View();
    }
}
