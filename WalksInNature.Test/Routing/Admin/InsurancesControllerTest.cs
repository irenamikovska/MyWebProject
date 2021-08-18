using MyTested.AspNetCore.Mvc;
using Xunit;

using WalksInNature.Areas.Admin.Controllers;
using WalksInNature.Models.Insurances;


namespace WalksInNature.Test.Routing.Admin
{
    public class InsurancesControllerTest
    {
        [Fact]
        public void GetAllShouldBeMapedWithCorrectModel()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Insurances/All")
                .To<InsurancesController>(c => c.All(With.Any<AllInsuranceQueryModel>()));

        [Fact]
        public void ChangeStatusShouldBeMappedCorrectly()
           => MyRouting
               .Configuration()
               .ShouldMap("/Admin/Insurances/ChangeStatus/SomeGuid")
               .To<InsurancesController>(c => c.ChangeStatus("SomeGuid"));


        [Fact]
        public void DeleteShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Insurances/Delete/SomeGuid")
                .To<InsurancesController>(x => x.Delete("SomeGuid"));
    }
}
