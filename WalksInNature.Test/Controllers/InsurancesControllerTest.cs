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

namespace WalksInNature.Test.Controllers
{
    public class InsurancesControllerTest
    {

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

        
        [Theory]
        [InlineData("01.01.2022", "03.01.2022", 2, 2000, "Aaaaaaaaaaaaaaaaaaaa")]
        public void PostAddShouldBeForAuthorizedUsersAndReturnRedirectWithValidModel(
            string startDate, string endDate, int numberOfPeople, int limit, string beneficiary)
            => MyController<InsurancesController>
                .Instance(controller => controller.WithUser())
                .Calling(x => x.Add(new InsuranceFormModel
                {
                    StartDate = DateTime.ParseExact(startDate, "dd.MM.yyyy", CultureInfo.InvariantCulture),
                    EndDate = DateTime.ParseExact(endDate, "dd.MM.yyyy", CultureInfo.InvariantCulture),
                    NumberOfPeople = numberOfPeople,
                    Limit = limit,
                    Beneficiary = beneficiary
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                    .WithSet<Insurance>(ins => ins
                        .Any(x =>
                            x.NumberOfPeople == numberOfPeople &&
                            x.Limit == limit &&
                            x.UserId == TestUser.Identifier)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("MyInsurances", "Insurances");
        
    }
}
