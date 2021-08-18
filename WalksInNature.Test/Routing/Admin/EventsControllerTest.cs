using MyTested.AspNetCore.Mvc;
using WalksInNature.Areas.Admin.Controllers;
using WalksInNature.Models.Events;
using Xunit;

namespace WalksInNature.Test.Routing.Admin
{
    public class EventsControllerTest
    {

        [Fact]
        public void GetAllShouldBeMapedWithCorrectModel()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Events/All")
                .To<EventsController>(c => c.All(With.Any<AllEventsQueryModel>()));

        [Fact]
        public void ChangeStatusShouldBeMappedCorrectly()
           => MyRouting
               .Configuration()
               .ShouldMap("/Admin/Events/ChangeStatus/1")
               .To<EventsController>(c => c.ChangeStatus(1));


        [Fact]
        public void DeleteShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Events/Delete/1")                    
                .To<EventsController>(x => x.Delete(1));
    }
}
