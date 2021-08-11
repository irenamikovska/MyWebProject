using MyTested.AspNetCore.Mvc;
using Xunit;
using WalksInNature.Controllers;
using WalksInNature.Models.Insurances;

namespace WalksInNature.Test.Routing
{
    public class InsurancesControllerTest
    {
        [Fact]
        public void GetAddShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Insurances/Add")
                .To<InsurancesController>(x => x.Add());

        [Fact]
        public void PostAddShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Insurances/Add")
                    .WithMethod(HttpMethod.Post))
                    .To<InsurancesController>(x => x.Add(With.Any<InsuranceFormModel>()));

        [Fact]
        public void GetMyInsurancesShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Insurances/MyInsurances")
                .To<InsurancesController>(x => x.MyInsurances());

        [Theory]
        [InlineData("SomeStringId")]
        public void GetDetailsShouldBeMapped(string someStringId)
           => MyRouting
               .Configuration()
               .ShouldMap("/Insurances/Details/SomeStringId")
               .To<InsurancesController>(x => x.Details(someStringId));

        
    }
}