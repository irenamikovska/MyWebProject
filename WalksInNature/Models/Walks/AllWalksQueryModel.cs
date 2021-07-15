﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WalksInNature.Models.Walks
{
    public class AllWalksQueryModel
    {        
        // for searching       

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }      
        public IEnumerable<WalkListingViewModel> Walks { get; set; }

        // for filtering by region
        public string Region { get; init; }
        public IEnumerable<string> Regions { get; set; }

        // for sorting
        public WalkSorting Sorting { get; init; }

        // for paging

        public const int WalksPerPage = 3;
        public int CurrentPage { get; init; } = 1;
        public int TotalWalks { get; set; }
    }
}
