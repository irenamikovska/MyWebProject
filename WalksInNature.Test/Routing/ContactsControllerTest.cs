using MyTested.AspNetCore.Mvc;
using Xunit;
using WalksInNature.Controllers;
using WalksInNature.Models.Contacts;

namespace WalksInNature.Test.Routing
{
    public class ContactsControllerTest
    {

        [Fact]
        public void GetIndexShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/Contacts/Index")
                .To<ContactsController>(c => c.Index());


        [Fact]
        public void PostIndexShouldBeMappedWithCorrectModel()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Contacts/Index")
                    .WithMethod(HttpMethod.Post))
                    .To<ContactsController>(x => x.Index(With.Any<ContactFormModel>()));

        [Fact]
        public void SuccessMessageShouldBeMappedCorrectly()
           => MyRouting
               .Configuration()
               .ShouldMap("/Contacts/SuccessMessage")
               .To<ContactsController>(c => c.SuccessMessage(With.Any<string>()));
    }
}
