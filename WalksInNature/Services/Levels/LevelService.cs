using System.Collections.Generic;
using System.Linq;
using WalksInNature.Data;

namespace WalksInNature.Services.Levels
{
    public class LevelService : ILevelService
    {
        private readonly WalksDbContext data;
        public LevelService(WalksDbContext data) => this.data = data;

        public IEnumerable<LevelServiceModel> GetLevels()
            => this.data.Levels
                .Select(x => new LevelServiceModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();

        public bool LevelExists(int levelId)
            => this.data.Levels.Any(x => x.Id == levelId);
    }
}
