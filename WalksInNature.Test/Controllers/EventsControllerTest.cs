using MyTested.AspNetCore.Mvc;
using WalksInNature.Controllers;
using Xunit;

namespace WalksInNature.Test.Controllers
{
    public class EventsControllerTest
    {
        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnView()
               => MyController<EventsController>
                   .Instance()
                   .Calling(x => x.Add())
                   .ShouldHave()
                   .ActionAttributes(attributes => attributes
                       .RestrictingForAuthorizedRequests())
                   .AndAlso()
                   .ShouldReturn()
                   .View();

        [Fact]
        public void DeleteShouldHaveRestrictionsForAuthorizedUsers()
            => MyController<EventsController>
                .Calling(c => c.Delete(With.Empty<int>()))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests());
    }
}
