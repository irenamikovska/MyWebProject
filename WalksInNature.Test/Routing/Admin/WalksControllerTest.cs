using MyTested.AspNetCore.Mvc;
using WalksInNature.Areas.Admin.Controllers;
using WalksInNature.Models.Walks;
using Xunit;

namespace WalksInNature.Test.Routing.Admin
{
    public class WalksControllerTest
    {

        [Fact]
        public void GetAllShouldBeMapedWithCorrectModel()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Walks/All")
                .To<WalksController>(c => c.All(With.Any<AllWalksQueryModel>()));

        [Fact]
        public void ChangeStatusShouldBeMappedCorrectly()
           => MyRouting
               .Configuration()
               .ShouldMap("/Admin/Walks/ChangeStatus/1")
               .To<WalksController>(c => c.ChangeStatus(1));


        [Fact]
        public void DeleteShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Walks/Delete/1")
                .To<WalksController>(x => x.Delete(1));
    }
}
