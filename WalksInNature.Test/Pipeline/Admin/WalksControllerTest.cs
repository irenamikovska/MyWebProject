using MyTested.AspNetCore.Mvc;
using System.Linq;
using Xunit;

using WalksInNature.Areas.Admin;
using WalksInNature.Areas.Admin.Controllers;
using WalksInNature.Models.Walks;
using WalksInNature.Data.Models;

using static WalksInNature.Test.Data.Walks;

namespace WalksInNature.Test.Pipeline.Admin
{
    public class WalksControllerTest
    {

        [Fact]
        public void ChangeStatusShouldChangeWalkAndRedirectToAll()
            => MyPipeline
                  .Configuration()
                   .ShouldMap(request => request
                    .WithPath("/Admin/Walks/ChangeStatus/1")
                     .WithUser(new[] { AdminConstants.AdministratorRoleName })
                     .WithAntiForgeryToken())
                   .To<WalksController>(c => c.ChangeStatus(1))
                   .Which(controller => controller
                    .WithData(GetWalk()))
                   .ShouldHave()
                    .Data(data => data
                         .WithSet<Walk>(set => set
                              .Any(x => x.Id == 1 && !x.IsPublic)))
                     .AndAlso()
                     .ShouldReturn()
                     .Redirect(redirect => redirect
                        .To<WalksController>(c => c.All(With.Any<AllWalksQueryModel>())));


        [Fact]
        public void DeleteShouldReturnCorrectViewAndModel()
             => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Admin/Walks/Delete/1")
                     .WithUser(new[] { AdminConstants.AdministratorRoleName })
                     .WithAntiForgeryToken())
                .To<WalksController>(c => c.Delete(1))
                .Which(controller => controller
                    .WithData(GetWalk(1, true)))
                .ShouldReturn()
                .Redirect(redirect => redirect
                        .To<WalksController>(c => c.All(With.Any<AllWalksQueryModel>())));
    }
}
