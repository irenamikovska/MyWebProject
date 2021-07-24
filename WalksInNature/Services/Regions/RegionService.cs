using System.Collections.Generic;
using System.Linq;
using WalksInNature.Data;

namespace WalksInNature.Services.Regions
{
    public class RegionService : IRegionService
    {
        private readonly WalksDbContext data;
        public RegionService(WalksDbContext data) => this.data = data;

        public IEnumerable<RegionServiceModel> GetRegions()
            => this.data.Regions
                .Select(x => new RegionServiceModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();

        public bool RegionExists(int regionId)
            => this.data.Regions.Any(x => x.Id == regionId);
    }
}
