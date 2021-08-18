using FluentAssertions;
using MyTested.AspNetCore.Mvc;
using WalksInNature.Controllers;
using WalksInNature.Data.Models;
using WalksInNature.Models.Events;
using Xunit;
using System.Linq;

using static WalksInNature.Test.Data.Users;
using static WalksInNature.Test.Data.Regions;
using static WalksInNature.Test.Data.Levels;
using static WalksInNature.Test.Data.Events;
using static WalksInNature.Test.Data.DataConstants;
using WalksInNature.Services.Events.Models;
using System.Collections.Generic;

namespace WalksInNature.Test.Controllers
{
    public class EventsControllerTest
    {

        [Fact]
        public void MyEventsShouldHaveRestrictionsForAuthorizedUsersAndReturnView()
           => MyController<EventsController>
                .Instance(controller => controller
                     .WithUser())                        
               .Calling(c => c.MyGuideEvents())
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<List<EventServiceModel>>());

        [Fact]
        public void DetailsShouldReturnViewWithCorrectModelAndEventId()
           => MyController<EventsController>
               .Instance(instance => instance
                   .WithData(GetEvent(TestIdInt, isPublic: true)))
               .Calling(c => c.Details(TestIdInt, With.Any<string>()))
               .ShouldReturn()
               .View(view => view
                   .WithModelOfType<EventDetailsServiceModel>()
                   .Passing(e => e.Id == TestIdInt));



        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnViewWithCorrectModel()
               => MyController<EventsController>
                   .Instance(controller => controller
                        .WithUser()
                            .WithData(GetGuide()))
                   .Calling(x => x.Add())
                   .ShouldHave()
                   .ActionAttributes(attributes => attributes
                       .RestrictingForAuthorizedRequests())
                   .AndAlso()
                   .ShouldReturn()
                   .View(view => view
                        .WithModelOfType<EventFormModel>());

        [Fact]
        public void PostAddShouldReturnViewWhenFormIsWithWrongFields()
            => MyController<EventsController>
                .Instance(controller => controller
                        .WithUser()
                        .WithData(GetGuide()))
                 .Calling(c => c.Add(new EventFormModel
                 {

                 }))
                .ShouldHave()
               .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .InvalidModelState()
                .Data(data => data
                     .WithSet<Event>(set => set.Should().BeEmpty()))
                 .AndAlso()
                 .ShouldReturn()
                 .View(view => view
                      .WithModelOfType<EventFormModel>());

        [Fact]
        public void PostAddShouldReturnRedirectWhenUserIsNotGuide()
          => MyController<EventsController>
              .Instance(controller => controller
                      .WithUser())
               .Calling(c => c.Add(new EventFormModel
               {

               }))
              .ShouldHave()
             .ActionAttributes(attributes => attributes
                 .RestrictingForAuthorizedRequests()
                  .RestrictingForHttpMethod(HttpMethod.Post))
                .AndAlso()
               .ShouldReturn()            
               .Redirect(r => r.To<GuidesController>(c => c.Become()));
                

        [Fact]
        public void PostEditShouldReturnRedirectWhenUserIsNotGuide()
         => MyController<EventsController>
             .Instance(controller => controller
                     .WithUser())
              .Calling(c => c.Edit(TestIdInt, new EventFormModel
              {

              }))
             .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForAuthorizedRequests()
                 .RestrictingForHttpMethod(HttpMethod.Post))
            .AndAlso()
            .ShouldReturn()
            .Redirect(r => r.To<GuidesController>(c => c.Become()));


        [Fact]
        public void PostEditShouldReturnViewWhenFormFieldsAreEmpty()
       => MyController<EventsController>
           .Instance(controller => controller
                   .WithUser()
                   .WithData(GetGuide())
                   .WithData(GetEvent(TestIdInt)))
            .Calling(c => c.Edit(TestIdInt, new EventFormModel
            {


            }))
           .ShouldHave()
           .ActionAttributes(attributes => attributes
              .RestrictingForAuthorizedRequests()
               .RestrictingForHttpMethod(HttpMethod.Post))
             .AndAlso()
             .ShouldReturn()
             .View(view => view                      
                      .WithModelOfType<EventFormModel>());


        [Fact]
        public void AddUserToEventShouldBeForAuthorizedUsersAndReturnView()
               => MyController<EventsController>
                   .Instance(controller => controller
                     .WithUser()
                     .WithData(GetEvent(1)))
                   .Calling(x => x.AddUser(1))
                   .ShouldHave()
                   .ActionAttributes(attributes => attributes
                       .RestrictingForAuthorizedRequests())
                   .AndAlso()
                   .ShouldReturn()
                   .Redirect(redirect => redirect
                   .To<EventsController>(c => c
                   .All(With.Any<AllEventsQueryModel>())));


        [Fact]
        public void DeleteShouldHaveRestrictionsForAuthorizedUsersAndReturnRedirect()
            => MyController<EventsController>
                .Instance(controller => controller
                     .WithUser()
                     .WithData(GetGuide())
                        .WithData(GetEvent(TestIdInt, true)))
                .Calling(c => c.Delete(TestIdInt))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                   .To<EventsController>(c => c
                   .All(With.Any<AllEventsQueryModel>())));

    }
}
