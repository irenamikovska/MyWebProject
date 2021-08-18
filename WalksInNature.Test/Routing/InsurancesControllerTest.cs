using MyTested.AspNetCore.Mvc;
using Xunit;
using WalksInNature.Controllers;
using WalksInNature.Models.Insurances;

namespace WalksInNature.Test.Routing
{
    public class InsurancesControllerTest
    {
        
        [Fact]
        public void GetMyInsurancesShouldBeMapped()
           => MyRouting
               .Configuration()
               .ShouldMap("/Insurances/MyInsurances")
               .To<InsurancesController>(x => x.MyInsurances());

        [Fact]
        public void GetDetailsShouldBeMapped()
           => MyRouting
               .Configuration()
               .ShouldMap("/Insurances/Details/SomeGuid")
               .To<InsurancesController>(x => x.Details("SomeGuid"));

        [Fact]
        public void GetAddShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Insurances/Add")
                .To<InsurancesController>(x => x.Add());

        [Fact]
        public void PostAddShouldBeMappedWithCorrectModel()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Insurances/Add")
                    .WithMethod(HttpMethod.Post))
                    .To<InsurancesController>(x => x.Add(With.Any<InsuranceFormModel>()));

        [Fact]
        public void GetEditShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Insurances/Edit/SomeGuid")
                .To<InsurancesController>(x => x.Edit("SomeGuid"));

        [Fact]
        public void PostEditShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Insurances/Edit/SomeGuid")
                    .WithMethod(HttpMethod.Post))
                    .To<InsurancesController>(x => x.Edit("SomeGuid"));


        [Fact]
        public void GetDeleteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Insurances/Delete/SomeGuid")
                .To<InsurancesController>(x => x.Delete("SomeGuid"));

    }
}