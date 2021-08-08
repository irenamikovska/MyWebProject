using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Linq;
using WalksInNature.Data;

namespace WalksInNature.Services.Regions
{
    public class RegionService : IRegionService
    {
        private readonly WalksDbContext data;
        private readonly IMapper mapper;
        public RegionService(WalksDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        } 

        public IEnumerable<RegionServiceModel> GetRegions()
            => this.data
                    .Regions
                    .ProjectTo<RegionServiceModel>(this.mapper.ConfigurationProvider)                
                    .ToList();

        public bool RegionExists(int regionId)
            => this.data.Regions.Any(x => x.Id == regionId);
    }
}
