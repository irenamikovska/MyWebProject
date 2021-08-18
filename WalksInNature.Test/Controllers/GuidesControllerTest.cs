using MyTested.AspNetCore.Mvc;
using System.Linq;
using WalksInNature.Controllers;
using WalksInNature.Data.Models;
using WalksInNature.Models.Guides;
using Xunit;

using static WalksInNature.WebConstants;

namespace WalksInNature.Test.Controllers
{
    public class GuidesControllerTest
    {
        [Fact]
        public void GetBecomeShouldBeForAuthorizedUsersAndReturnView()
            => MyController<GuidesController>
                .Instance()
                .Calling(x => x.Become())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData("Guide", "+359-888-505-808")]
        public void PostBecomeShouldBeForAuthorizedUsersAndReturnRedirectWithValidModel(
            string guideName, string phoneNumber)
            => MyController<GuidesController>
                .Instance(controller => controller.WithUser())
                .Calling(x => x.Become(new BecomeGuideFormModel
                {
                    Name = guideName,
                    PhoneNumber = phoneNumber
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                    .WithSet<Guide>(guides => guides
                        .Any(x =>
                            x.Name == guideName &&
                            x.PhoneNumber == phoneNumber &&
                            x.UserId == TestUser.Identifier)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("All", "Events");
    }
}
