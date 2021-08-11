using MyTested.AspNetCore.Mvc;
using Xunit;
using WalksInNature.Controllers;
using WalksInNature.Models.Events;

namespace WalksInNature.Test.Routing
{
    public class EventsControllerTest
    {
        [Fact]
        public void GetAllShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Events/All/")
                .To<EventsController>(x => x.All(With.Any<AllEventsQueryModel>()));

        [Theory]
        [InlineData("SomeName")]
        public void GetDetailsShouldBeMapped(string someName)
            => MyRouting
                .Configuration()
                .ShouldMap("/Events/Details/1/SomeName")
                .To<EventsController>(x => x.Details(1, someName));

        [Fact]
        public void GetAddShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Events/Add")
                .To<EventsController>(x => x.Add());

        [Fact]
        public void PostAddShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Events/Add")
                    .WithMethod(HttpMethod.Post))
                    .To<EventsController>(x => x.Add(With.Any<EventFormModel>()));

        [Fact]
        public void GetEditShouldBeMapped()
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
        public void GetMyEventsShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Events/MyEvents")
                .To<EventsController>(x => x.MyEvents());

        [Fact]
        public void AddUserToEventShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Events/AddUser/1")
                .To<EventsController>(x => x.AddUser(1));

        [Fact]
        public void RemoveUserByEventShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Events/RemoveUser/1")
                .To<EventsController>(x => x.RemoveUser(1));

        [Fact]
        public void GetDeleteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Events/Delete/1")
                .To<EventsController>(x => x.Delete(1));
    }
}
