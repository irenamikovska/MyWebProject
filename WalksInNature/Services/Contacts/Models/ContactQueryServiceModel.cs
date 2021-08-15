using System.Collections.Generic;

namespace WalksInNature.Services.Contacts.Models
{
    public class ContactQueryServiceModel
    {
        public int CurrentPage { get; init; }
        public int MessagesPerPage { get; init; }
        public int TotalMessages { get; init; }
        public IEnumerable<ContactServiceModel> Messages { get; init; }

    }
}
