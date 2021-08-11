using MyTested.AspNetCore.Mvc;
using System.Linq;
using FluentAssertions;
using Xunit;
using WalksInNature.Controllers;
using WalksInNature.Data.Models;
using WalksInNature.Models.Walks;

using static WalksInNature.WebConstants;
using static WalksInNature.Test.Data.Walks;
using WalksInNature.Services.Walks.Models;

namespace WalksInNature.Test.Controllers
{
    public class WalksControllerTest
    {
        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnView()
               => MyController<WalksController>
                   .Instance()
                   .Calling(x => x.Add())
                   .ShouldHave()
                   .ActionAttributes(attributes => attributes
                       .RestrictingForAuthorizedRequests())
                   .AndAlso()
                   .ShouldReturn()
                   .View();
               
        [Theory]
        [InlineData("walkName", "imageUrl", "startPoint", 1, 1, "description")]
        public void PostAddShouldBeForAuthorizedUsersAndReturnRedirectWithValidModel(
            string walkName, string imageUrl, string startPoint, int regionId, int levelId, string description)
            => MyController<WalksController>
                .Instance(controller => controller.WithUser())
                .Calling(x => x.Add(new WalkFormModel
                {
                    Name = walkName,
                    ImageUrl = imageUrl,
                    StartPoint = startPoint,
                    RegionId = regionId,
                    LevelId = levelId,
                    Description = description
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                    .WithSet<Walk>(walks => walks
                        .Any(x =>
                            x.Name == walkName &&
                            x.ImageUrl == imageUrl &&
                            x.StartPoint == startPoint &&
                            x.RegionId == regionId &&
                            x.LevelId == levelId &&
                            x.Description == description &&
                            x.AddedByUserId == TestUser.Identifier)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Details", "Walks");

        [Fact]
        public void GetEditShouldBeForAuthorizedUsersAndReturnView()
              => MyController<WalksController>
                  .Instance(controller => controller.WithUser())
                  .Calling(x => x.Edit(1))
                  .ShouldHave()
                  .ActionAttributes(attributes => attributes
                      .RestrictingForAuthorizedRequests())
                  .AndAlso()
                  .ShouldReturn()
                  .View();

        [Fact]
        public void MyWalksShouldHaveRestrictionsForAuthorizedUsers()
           => MyController<WalksController>
               .Calling(c => c.MyWalks())
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForAuthorizedRequests());

        [Fact]
        public void DeleteShouldHaveRestrictionsForAuthorizedUsers()
            => MyController<WalksController>
                .Calling(c => c.Delete(With.Empty<int>()))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests());

    }
}
