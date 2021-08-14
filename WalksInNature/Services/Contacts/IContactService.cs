using WalksInNature.Models.Contacts;

namespace WalksInNature.Services.Contacts
{
    public interface IContactService
    {
        int EntryContactForm(ContactFormModel inputModel);
        ContactConfirmMessageViewModel ConfirmMessage(string senderName);
    }
}
