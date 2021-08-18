using MyTested.AspNetCore.Mvc;
using WalksInNature.Controllers;
using WalksInNature.Models.Walks;
using Xunit;

namespace WalksInNature.Test.Routing
{
    public class WalksControllerTest
    {

        [Fact]
        public void GetAllShouldBeMappedWithCorrectModel()
            => MyRouting
                .Configuration()
                .ShouldMap("/Walks/All/")
                .To<WalksController>(x => x.All(With.Any<AllWalksQueryModel>()));

        [Fact]
        public void GetMyWalksShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Walks/MyWalks")
                .To<WalksController>(x => x.MyWalks());


        [Fact]
        public void GetDetailsShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Walks/Details/1/SomeName")
                .To<WalksController>(x => x.Details(1, "SomeName"));
        
        [Fact]
        public void GetAddShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Walks/Add")
                .To<WalksController>(x => x.Add());

        [Fact]
        public void PostAddShouldBeMappedWithCorrectModel()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Walks/Add")
                    .WithMethod(HttpMethod.Post))
                    .To<WalksController>(x => x.Add(With.Any<WalkFormModel>()));        

        [Fact]
        public void GetEditShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Walks/Edit/1")
                .To<WalksController>(x => x.Edit(1));
        
        [Fact]       
        public void PostEditShouldBeRoutedCorrectly()
           => MyRouting
               .Configuration()
               .ShouldMap(request => request
                   .WithPath("/Walks/Edit/1")
                   .WithMethod(HttpMethod.Post))                  
               .To<WalksController>(x => x.Edit(1));
         

        [Fact]
        public void AddLikeShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Walks/AddLike/1")
                .To<WalksController>(x => x.AddLike(1));

        [Fact]
        public void GetDeleteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Walks/Delete/1")
                .To<WalksController>(x => x.Delete(1));

    }
}
