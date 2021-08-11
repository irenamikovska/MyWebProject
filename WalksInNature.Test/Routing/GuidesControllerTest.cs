using MyTested.AspNetCore.Mvc;
using WalksInNature.Controllers;
using WalksInNature.Models.Guides;
using Xunit;

namespace WalksInNature.Test.Routing
{
    public class GuidesControllerTest
    {
        [Fact]
        public void GetBecomeShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Guides/Become")
                .To<GuidesController>(x => x.Become());

        [Fact]
        public void PostBecomeShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Guides/Become")
                    .WithMethod(HttpMethod.Post))
                .To<GuidesController>(x => x.Become(With.Any<BecomeGuideFormModel>()));
    }
}
