using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WalksInNature.Services.Events.Models;

namespace WalksInNature.Models.Events
{
    public class AllEventsQueryModel
    {        
        // for searching       

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }      
        public IEnumerable<EventServiceModel> Events { get; set; }

        // for filtering by region
        public string GuideName { get; init; }            
        public IEnumerable<string> Guides { get; set; }

        // for sorting
        public EventSorting Sorting { get; init; }

        // for paging

        public const int EventsPerPage = 3;
        public int CurrentPage { get; init; } = 1;
        public int TotalEvents { get; set; }
    }
}
