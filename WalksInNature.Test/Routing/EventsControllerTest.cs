using MyTested.AspNetCore.Mvc;
using Xunit;
using WalksInNature.Controllers;
using WalksInNature.Models.Events;

namespace WalksInNature.Test.Routing
{
    public class EventsControllerTest
    {
        [Fact]
        public void GetAllShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/Events/All/")
                .To<EventsController>(x => x.All(With.Any<AllEventsQueryModel>()));

        [Fact]
        public void GetMyGuideEventsShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/Events/MyGuideEvents")
                .To<EventsController>(x => x.MyGuideEvents());

        [Fact]
        public void GetMyUserEventsShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/Events/MyUserEvents")
                .To<EventsController>(x => x.MyUserEvents());

        [Fact]
        public void GetDetailsShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/Events/Details/1/SomeName")
                .To<EventsController>(x => x.Details(1, "SomeName"));

        [Fact]
        public void GetAddShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/Events/Add")
                .To<EventsController>(x => x.Add());

        [Fact]
        public void PostAddShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Events/Add")
                    .WithMethod(HttpMethod.Post))
                    .To<EventsController>(x => x.Add(With.Any<EventFormModel>()));

        [Fact]
        public void GetEditShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/Events/Edit/1")
                .To<EventsController>(x => x.Edit(1));

        [Fact]
        public void PostEditShouldBeRoutedCorrectly()
           => MyRouting
               .Configuration()
               .ShouldMap(request => request
                   .WithPath("/Events/Edit/1")
                   .WithMethod(HttpMethod.Post))
               .To<EventsController>(x => x.Edit(1));


        [Fact]
        public void AddUserToEventShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/Events/AddUser/1")
                .To<EventsController>(x => x.AddUser(1));

        [Fact]
        public void RemoveUserByEventShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/Events/RemoveUser/1")
                .To<EventsController>(x => x.RemoveUser(1));

        [Fact]
        public void GetDeleteShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/Events/Delete/1")
                .To<EventsController>(x => x.Delete(1));
    }
}
