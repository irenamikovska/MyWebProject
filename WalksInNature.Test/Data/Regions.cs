using System.Collections.Generic;
using System.Linq;
using WalksInNature.Data.Models;

namespace WalksInNature.Test.Data
{
    public class Regions
    {
        public static IEnumerable<Region> GetRegions(int count = 25)
               => Enumerable.Range(1, count).Select(i => new Region()
               {
                   
               });
    }
}
