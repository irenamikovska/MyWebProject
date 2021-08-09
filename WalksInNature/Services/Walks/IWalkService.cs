using System.Collections.Generic;
using WalksInNature.Models.Walks;
using WalksInNature.Services.Walks.Models;

namespace WalksInNature.Services.Walks
{
    public interface IWalkService
    {
        WalkQueryServiceModel All(
                    string region = null,
                    string searchTerm = null,
                    WalkSorting sorting = WalkSorting.DateCreated,
                    int currentPage = 1,
                    int walksPerPage = int.MaxValue,
                    bool publicOnly = true);

        IEnumerable<LatestWalkServiceModel> Latest();
        IEnumerable<string> AllWalkRegions();

        WalkDetailsServiceModel GetDetails(int walkId);

        bool AddUserToWalk(string userId, int walkId);
        IEnumerable<WalkServiceModel> WalksByUser(string userId);        

        int Create(string name, string imageUrl, string startPoint,
            int regionId, int levelId, string description, string userId);

        void ChangeStatus(int walkId);

        bool Edit(int id, string name, string imageUrl, string startPoint,
            int regionId, int levelId, string description, bool isPublic);

        void Delete(int id);
    }
}

