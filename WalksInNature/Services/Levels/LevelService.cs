using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Linq;
using WalksInNature.Data;

namespace WalksInNature.Services.Levels
{
    public class LevelService : ILevelService
    {
        private readonly WalksDbContext data;
        private readonly IMapper mapper;
        public LevelService(WalksDbContext data, IMapper mapper) 
        {
            this.data = data;
            this.mapper = mapper;
        }

        public IEnumerable<LevelServiceModel> GetLevels()
            => this.data
                .Levels
                .ProjectTo<LevelServiceModel>(this.mapper.ConfigurationProvider)                
                .ToList();

        public bool LevelExists(int levelId)
            => this.data.Levels.Any(x => x.Id == levelId);
    }
}
