using System.Collections.Generic;

namespace WalksInNature.Models.Home
{
    public class IndexViewModel
    {
        public int TotalWalks { get; init; }
        public int TotalUsers { get; init; }
        public int TotalEvents { get; init; }
        public List<WalkIndexViewModel> Walks { get; set; }
    }
}
