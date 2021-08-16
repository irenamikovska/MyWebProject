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
using static WalksInNature.Test.Data.DataConstants;
using static WalksInNature.Test.Data.Regions;
using static WalksInNature.Test.Data.Levels;
using System.Collections.Generic;

namespace WalksInNature.Test.Controllers
{
    public class WalksControllerTest
    {

        [Fact]
        public void MyWalksShouldHaveRestrictionsForAuthorizedUsersAndReturnView()
           => MyController<WalksController>
                .Instance(controller => controller
                     .WithUser())
               .Calling(c => c.MyWalks())
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<List<WalkServiceModel>>());


        [Fact]
        public void DetailsShouldReturnViewWithCorrectModelAndWalkId()
            => MyController<WalksController>
                .Instance(instance => instance
                    .WithData(GetWalk(1, IsPublic: true)))
                .Calling(c => c.Details(1, With.Any<string>()))
                .ShouldReturn()
                .View(view => view 
                    .WithModelOfType<WalkDetailsServiceModel>()
                    .Passing(walk => walk.Id == 1));

        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnViewWithCorrectModel()
               => MyController<WalksController>
                   .Instance()
                   .Calling(x => x.Add())
                   .ShouldHave()
                   .ActionAttributes(attributes => attributes
                       .RestrictingForAuthorizedRequests())
                   .AndAlso()
                   .ShouldReturn()
                   .View(view => view
                        .WithModelOfType<WalkFormModel>());

        [Fact]
        public void PostAddShouldReturnViewWhenFormIsWithoutFields()
           => MyController<WalksController>
               .Instance(controller => controller
                       .WithUser())
                .Calling(c => c.Add(new WalkFormModel
                {

                }))
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                  .RestrictingForAuthorizedRequests()
                   .RestrictingForHttpMethod(HttpMethod.Post))
               .InvalidModelState()
               .Data(data => data
                    .WithSet<Walk>(set => set.Should().BeEmpty()))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                     .WithModelOfType<WalkFormModel>());

        

        [Fact]
        public void PostEditShouldReturnViewWhenFormIsWithoutFields()
         => MyController<WalksController>
             .Instance(controller => controller
                     .WithUser())
              .Calling(c => c.Edit(1, new WalkFormModel
              {

              }))
             .ShouldHave()
             .ActionAttributes(attributes => attributes
                .RestrictingForAuthorizedRequests()
                 .RestrictingForHttpMethod(HttpMethod.Post))
               .AndAlso()
              .ShouldReturn()
              .View(view => view
                     .WithModelOfType<WalkFormModel>());


        [Fact]
        public void AddLikeShouldBeForAuthorizedUsersAndReturnViewWithCorrectModel()
               => MyController<WalksController>
                   .Instance(controller => controller
                     .WithUser()
                     .WithData(GetWalk(1)))
                   .Calling(x => x.AddLike(1))
                   .ShouldHave()
                   .ActionAttributes(attributes => attributes
                       .RestrictingForAuthorizedRequests())                       
                   .AndAlso()
                   .ShouldReturn()
                   .Redirect();


        [Fact]
        public void DeleteShouldHaveRestrictionsForAuthorizedUsersAndReturnRedirect()
            => MyController<WalksController>
                .Instance(controller => controller
                     .WithUser()
                     .WithData(GetWalk(1)))
                .Calling(c => c.Delete(1))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect();

    }
}
