using System.Collections.Generic;

namespace WalksInNature.Services.Levels
{
    public interface ILevelService
    {
        IEnumerable<LevelServiceModel> GetLevels();        
        bool LevelExists(int levelId);
    }
}
