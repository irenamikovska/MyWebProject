using System.Collections.Generic;
using WalksInNature.Models.Walks;

namespace WalksInNature.Services.Walks
{
    public interface IWalkService
    {
        WalkQueryServiceModel All(string region, string searchTerm, WalkSorting sorting, int currentPage, int walksPerPage);

        IEnumerable<string> AllWalkRegions();

    }
}
