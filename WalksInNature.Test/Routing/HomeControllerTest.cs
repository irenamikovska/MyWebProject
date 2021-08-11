using WalksInNature.Controllers;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace WalksInNature.Test.Routing
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexRouteShouldBeMapped()
           => MyRouting
               .Configuration()
               .ShouldMap("/")
               .To<HomeController>(c => c.Index());

        [Fact]
        public void ErrorRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Home/Error")
                .To<HomeController>(c => c.Error());


        [Fact]
        public void UsefulRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Home/Useful")
                .To<HomeController>(c => c.Useful());
    }
}
