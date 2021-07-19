﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WalksInNature.Models.Events
{
    public class AllEventsQueryModel
    {        
        // for searching       

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }      
        public IEnumerable<EventListingViewModel> Events { get; set; }

        // for filtering by region
        public string Date { get; init; }         
        public IEnumerable<string> Dates { get; set; }

        // for sorting
        public EventSorting Sorting { get; init; }

        // for paging

        public const int EventsPerPage = 3;
        public int CurrentPage { get; init; } = 1;
        public int TotalEvents { get; set; }
    }
}