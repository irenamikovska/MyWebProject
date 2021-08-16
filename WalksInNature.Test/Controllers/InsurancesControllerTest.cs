using MyTested.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WalksInNature.Controllers;
using WalksInNature.Data.Models;
using WalksInNature.Models.Insurances;
using WalksInNature.Services.Insurances.Models;
using Xunit;

using static WalksInNature.WebConstants;
using static WalksInNature.Test.Data.Insurances;
using static WalksInNature.Test.Data.DataConstants;
using FluentAssertions;

namespace WalksInNature.Test.Controllers
{
    public class InsurancesControllerTest
    {


        [Fact]
        public void MyInsurancesShouldHaveRestrictionsForAuthorizedUsersAndReturnView()
           => MyController<InsurancesController>
                .Instance(controller => controller
                     .WithUser())
               .Calling(c => c.MyInsurances())
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<List<InsuranceServiceModel>>());


        [Fact]
        public void DetailsShouldReturnViewWithCorrectModelAndInsuranceId()
            => MyController<InsurancesController>
                .Instance(instance => instance
                    .WithData(GetInsurance(TestIdGuid, isPaid: false)))
                .Calling(c => c.Details(TestIdGuid))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<InsuranceDetailsServiceModel>()
                    .Passing(ins => ins.Id == TestIdGuid));


        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnView()
           => MyController<InsurancesController>
               .Instance()
               .Calling(x => x.Add())
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .View();


        [Fact]
        public void PostAddShouldBeForAuthorizedUsersAndReturnRedirect()
           => MyController<InsurancesController>
               .Instance(controller => controller
                       .WithUser())
                .Calling(c => c.Add(new InsuranceFormModel
                {

                }))
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                  .RestrictingForAuthorizedRequests()
                   .RestrictingForHttpMethod(HttpMethod.Post))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                       .To<InsurancesController>(c => c
                       .MyInsurances()));


        [Fact]
        public void PostEditShouldReturnRedirectWhenFormIsWithoutFields()
         => MyController<InsurancesController>
             .Instance(controller => controller
                     .WithUser()
                        .WithData(GetInsurance(TestIdGuid, false)))
              .Calling(c => c.Edit(TestIdGuid, new InsuranceEditFormModel
              {

              }))
             .ShouldHave()
             .ActionAttributes(attributes => attributes
                .RestrictingForAuthorizedRequests()
                 .RestrictingForHttpMethod(HttpMethod.Post))
               .AndAlso()
              .ShouldReturn()
              .Redirect(redirect => redirect
                       .To<InsurancesController>(c => c
                       .Details(TestIdGuid)));


        [Fact]
        public void DeleteShouldHaveRestrictionsForAuthorizedUsersAndReturnUnauthorizedIfUserIsWrong()
            => MyController<InsurancesController>
                .Instance(controller => controller
                     .WithUser(u => u.WithIdentifier("WrongUser"))
                     .WithData(GetInsurance(TestIdGuid, false)))
                .Calling(c => c.Delete(TestIdGuid))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Unauthorized();

    }
}
