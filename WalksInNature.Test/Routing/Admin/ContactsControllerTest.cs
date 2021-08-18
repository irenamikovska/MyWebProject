using MyTested.AspNetCore.Mvc;
using WalksInNature.Areas.Admin.Controllers;
using WalksInNature.Models.Contacts;
using Xunit;

namespace WalksInNature.Test.Routing.Admin
{
    public class ContactsControllerTest
    {

        [Fact]
        public void GetAllShouldBeMapedWithCorrectModel()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Contacts/All")
                .To<ContactsController>(c => c.All(With.Any<AllMessagesQueryModel>()));


        [Fact]
        public void GetDetailsShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Contacts/Details/1")
                .To<ContactsController>(x => x.Details(1));

        [Fact]
        public void ChangeStatusShouldBeMappedCorrectly()
           => MyRouting
               .Configuration()
               .ShouldMap("/Admin/Contacts/ChangeStatus/1")
               .To<ContactsController>(c => c.ChangeStatus(1));
    }
}
