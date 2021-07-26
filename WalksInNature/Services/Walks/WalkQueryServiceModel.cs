using System.Collections.Generic;

namespace WalksInNature.Services.Walks
{
    public class WalkQueryServiceModel
    {
        public int CurrentPage { get; init; }
        public int WalksPerPage { get; init; }
        public int TotalWalks { get; init; }
        public IEnumerable<WalkServiceModel> Walks { get; init; }

    }
}
