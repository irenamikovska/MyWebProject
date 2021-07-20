using WalksInNature.Models.Walks;

namespace WalksInNature.Models.Api.Walks
{
    public class AllWalksApiRequestModel
    {
        // for searching      
        public string SearchTerm { get; init; }       

        // for filtering by region
        public string Region { get; init; }      

        // for sorting
        public WalkSorting Sorting { get; init; }

        // for paging
        public int WalksPerPage { get; init; } = 10;
        public int CurrentPage { get; init; } = 1;
       
    }
}
