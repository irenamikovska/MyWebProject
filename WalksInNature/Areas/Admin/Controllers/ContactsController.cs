using Microsoft.AspNetCore.Mvc;
using WalksInNature.Models.Contacts;
using WalksInNature.Services.Contacts;

namespace WalksInNature.Areas.Admin.Controllers
{
    public class ContactsController : AdminController
    {
        private readonly IContactService contactService;
        public ContactsController(IContactService contactService) => this.contactService = contactService;        

        public IActionResult All([FromQuery] AllMessagesQueryModel query)
        {
            var queryResult = this.contactService.All(
                    query.Sender,
                    query.SearchTerm,
                    query.Sorting,
                    query.CurrentPage,
                    AllMessagesQueryModel.MessagesPerPage);

            var senders = this.contactService.AllSenders();

            query.Senders = senders;
            query.TotalMessages = queryResult.TotalMessages;
            query.Messages = queryResult.Messages;

            return View(query);
        }
        public IActionResult Details(int id)
        {
            var message = this.contactService.GetDetails(id);

            if (message == null) 
            {
                return this.NotFound();
            }

            return this.View(message);           
        }
        public IActionResult ChangeStatus(int id)
        {
            this.contactService.ChangeStatus(id);

            return RedirectToAction(nameof(All));
        }

    }
}
