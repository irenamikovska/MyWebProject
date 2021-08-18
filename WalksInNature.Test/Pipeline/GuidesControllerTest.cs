using MyTested.AspNetCore.Mvc;
using System.Linq;
using WalksInNature.Controllers;
using WalksInNature.Data.Models;
using WalksInNature.Models.Events;
using WalksInNature.Models.Guides;
using Xunit;

using static WalksInNature.WebConstants;

namespace WalksInNature.Test.Pipeline
{
    public class GuidesControllerTest
    {
        [Fact]
        public void GetBecomeShouldBeForAuthorizedUsersAndReturnView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Guides/Become")
                    .WithUser())
                .To<GuidesController>(x => x.Become())
                .Which()
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData("Guide", "+359-888-505-808")]
        public void PostBecomeShouldBeForAuthorizedUsersAndReturnRedirectWithValidModel(string guideName, string phoneNumber)
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Guides/Become")
                    .WithMethod(HttpMethod.Post)
                    .WithFormFields(new
                    {
                        Name = guideName,
                        PhoneNumber = phoneNumber
                    })
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<GuidesController>(c => c.Become(new BecomeGuideFormModel
                {
                    Name = guideName,
                    PhoneNumber = phoneNumber
                }))
                .Which()
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                    .WithSet<Guide>(guides => guides
                        .Any(d =>  d.Name == guideName &&
                                   d.PhoneNumber == phoneNumber &&
                                   d.UserId == TestUser.Identifier)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<EventsController>(c => c.All(With.Any<AllEventsQueryModel>())));
    }
}
