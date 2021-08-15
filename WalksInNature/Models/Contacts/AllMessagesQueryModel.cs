using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WalksInNature.Services.Contacts.Models;

namespace WalksInNature.Models.Contacts
{
    public class AllMessagesQueryModel
    {
        // for searching       

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }
        public IEnumerable<ContactServiceModel> Messages { get; set; }

        // for filtering by sender name
        public string Sender { get; init; }
        public IEnumerable<string> Senders { get; set; }

        // for sorting
        public MessageSorting Sorting { get; init; }

        // for paging

        public const int MessagesPerPage = 10;
        public int CurrentPage { get; init; } = 1;
        public int TotalMessages { get; set; }
    }
}
