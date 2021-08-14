using Microsoft.AspNetCore.Mvc;
using WalksInNature.Models.Contacts;
using WalksInNature.Services.Contacts;

namespace WalksInNature.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IContactService contactService;

        public ContactsController(IContactService contactService)
        {
            this.contactService = contactService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Index(ContactFormModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            this.contactService.EntryContactForm(inputModel);
            var name = inputModel.Name;           

            return this.RedirectToAction(nameof(this.SuccessMessage), new { name });
        }

        public IActionResult SuccessMessage(string name)
        {
            var viewModel = this.contactService.ConfirmMessage(name);

            return this.View(viewModel);
        }
    }
}
