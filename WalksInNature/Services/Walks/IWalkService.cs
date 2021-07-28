using System.Collections.Generic;
using WalksInNature.Models.Walks;

namespace WalksInNature.Services.Walks
{
    public interface IWalkService
    {
        WalkQueryServiceModel All(string region, string searchTerm, WalkSorting sorting, int currentPage, int walksPerPage);

        IEnumerable<string> AllWalkRegions();

        WalkDetailsServiceModel GetDetails(int walkId);

        IEnumerable<WalkServiceModel> WalksByUser(string userId);        

        int Create(string name, string imageUrl, string startPoint,
            int regionId, int levelId, string description, string userId);

        bool Edit(int id, string name, string imageUrl, string startPoint,
            int regionId, int levelId, string description);
    }
}

