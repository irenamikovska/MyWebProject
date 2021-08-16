using MyTested.AspNetCore.Mvc;
using System.Linq;
using Xunit;

using WalksInNature.Areas.Admin;
using WalksInNature.Areas.Admin.Controllers;
using WalksInNature.Data.Models;
using WalksInNature.Models.Contacts;

using static WalksInNature.Test.Data.Contacts;


namespace WalksInNature.Test.Pipeline.Admin
{
    public class ContactsControllerTest
    {
        [Fact]
        public void ChangeStatusShouldChangeMessageAndRedirectToAll()
              => MyPipeline
                    .Configuration()
                     .ShouldMap(request => request
                      .WithPath("/Admin/Contacts/ChangeStatus/1")
                       .WithUser(new[] { AdminConstants.AdministratorRoleName })
                       .WithAntiForgeryToken())
                     .To<ContactsController>(c => c.ChangeStatus(1))
                     .Which(controller => controller
                      .WithData(GetContact()))
                     .ShouldHave()
                      .Data(data => data
                           .WithSet<ContactForm>(set => set
                                .Any(x => x.Id == 1 && !x.IsReplied)))
                       .AndAlso()
                       .ShouldReturn()
                       .Redirect(redirect => redirect
                          .To<ContactsController>(c => c.All(With.Any<AllMessagesQueryModel>())));
    }
}
