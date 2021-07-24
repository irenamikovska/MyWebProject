using System.Collections.Generic;

namespace WalksInNature.Services.Regions
{
    public interface IRegionService
    {
        IEnumerable<RegionServiceModel> GetRegions();
        bool RegionExists(int regionId);
    }
}
