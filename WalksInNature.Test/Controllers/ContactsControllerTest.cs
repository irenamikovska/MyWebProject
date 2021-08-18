using MyTested.AspNetCore.Mvc;
using Xunit;

using WalksInNature.Areas.Admin.Controllers;
using WalksInNature.Areas.Admin;
using WalksInNature.Data.Models;

using static WalksInNature.Test.Data.Contacts;
using System.Linq;
using WalksInNature.Models.Contacts;
using FluentAssertions;

namespace WalksInNature.Test.Controllers
{
    public class ContactsControllerTest
    {
        [Fact]
        public void ControllerShouldBeInAdminArea()
            => MyController<ContactsController>
                .ShouldHave()            
                .Attributes(attrs => attrs
                    .SpecifyingArea(AdminConstants.AreaName)
                    .RestrictingForAuthorizedRequests(AdminConstants.AdministratorRoleName));

        [Fact]
        public void ChangeVisibilityShouldChangeArticleAndRedirectToAll()
            => MyController<ContactsController>
                .Instance(instance => instance
                    .WithData(GetContacts(1)))
                .Calling(c => c.ChangeStatus(1))
                .ShouldHave()
                .Data(data => data
                    .WithSet<ContactForm>(set =>
                    {
                        var message = set.FirstOrDefault(a => a.IsReplied);
                       
                        
                    }))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<ContactsController>(c => c.All(With.Any<AllMessagesQueryModel>())));


    }
}
