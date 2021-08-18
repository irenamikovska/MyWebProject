using System.Collections.Generic;
using System.Linq;
using WalksInNature.Data.Models;

namespace WalksInNature.Test.Data
{
    public class Levels
    {
        public static IEnumerable<Level> GetLevels(int count = 3)
               => Enumerable.Range(1, count).Select(i => new Level()
               {

               });
    }
}
