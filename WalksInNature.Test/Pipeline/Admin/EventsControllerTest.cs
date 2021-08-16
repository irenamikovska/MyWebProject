using MyTested.AspNetCore.Mvc;
using System.Linq;
using Xunit;

using WalksInNature.Areas.Admin;
using WalksInNature.Areas.Admin.Controllers;
using WalksInNature.Data.Models;
using WalksInNature.Models.Events;

using static WalksInNature.Test.Data.Events;


namespace WalksInNature.Test.Pipeline.Admin
{
    public class EventsControllerTest
    {
        [Fact]
        public void ChangeStatusShouldChangeEventAndRedirectToAll()
               => MyPipeline
                     .Configuration()
                      .ShouldMap(request => request
                       .WithPath("/Admin/Events/ChangeStatus/1")
                        .WithUser(new[] { AdminConstants.AdministratorRoleName })
                        .WithAntiForgeryToken())
                      .To<EventsController>(c => c.ChangeStatus(1))
                      .Which(controller => controller
                       .WithData(GetEvent()))
                      .ShouldHave()
                       .Data(data => data
                            .WithSet<Event>(set => set
                                 .Any(x => x.Id == 1 && !x.IsPublic)))
                        .AndAlso()
                        .ShouldReturn()
                        .Redirect(redirect => redirect
                           .To<EventsController>(c => c.All(With.Any<AllEventsQueryModel>())));


        [Fact]
        public void DeleteShouldReturnCorrectViewAndModel()
             => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Admin/Events/Delete/1")
                     .WithUser(new[] { AdminConstants.AdministratorRoleName })
                     .WithAntiForgeryToken())
                .To<EventsController>(c => c.Delete(1))
                .Which(controller => controller
                    .WithData(GetEvent(1, true)))
                .ShouldReturn()
                .Redirect(redirect => redirect
                        .To<EventsController>(c => c.All(With.Any<AllEventsQueryModel>())));
    }
}
