using System.Collections.Generic;
using WalksInNature.Models.Contacts;
using WalksInNature.Services.Contacts.Models;

namespace WalksInNature.Services.Contacts
{
    public interface IContactService
    {
       
        ContactQueryServiceModel All(
                   string sender = null,
                   string searchTerm = null,
                   MessageSorting sorting = MessageSorting.DateCreated,
                   int currentPage = 1,
                   int messagesPerPage = int.MaxValue);

        IEnumerable<string> AllSenders();
        ContactDetailsServiceModel GetDetails(int messageId);
        int EntryContactForm(ContactFormModel inputModel);
        ContactConfirmMessageViewModel ConfirmMessage(string senderName);
        void ChangeStatus(int messageId);
    }
}
