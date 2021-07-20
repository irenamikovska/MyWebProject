using System.Collections.Generic;
using System.Linq;
using WalksInNature.Data;
using WalksInNature.Models.Walks;

namespace WalksInNature.Services.Walks
{
    public class WalkService : IWalkService
    {
        private readonly WalksDbContext data;
        public WalkService(WalksDbContext data) => this.data = data;

        public WalkQueryServiceModel All(string region, string searchTerm, WalkSorting sorting, int currentPage, int walksPerPage)
        {
            var walksQuery = this.data.Walks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(region))
            {
                walksQuery = walksQuery.Where(x => x.Region.Name == region);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                walksQuery = walksQuery.Where(x =>
                    x.Name.ToLower().Contains(searchTerm.ToLower()) ||
                    x.Region.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            walksQuery = sorting switch
            {
                WalkSorting.Name => walksQuery.OrderBy(x => x.Name),
                WalkSorting.Level => walksQuery.OrderByDescending(x => x.Level.Id),
                WalkSorting.DateCreated or _ => walksQuery.OrderByDescending(x => x.Id)
            };

            var totalWalks = walksQuery.Count();

            var walks = walksQuery
                .Skip((currentPage - 1) * walksPerPage)
                .Take(walksPerPage)
                .Select(x => new WalkServiceModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ImageUrl = x.ImageUrl,
                    Region = x.Region.Name,
                    Level = x.Level.Name
                })
                .ToList();

            return new WalkQueryServiceModel
            {
                TotalWalks = totalWalks,
                WalksPerPage = walksPerPage,
                CurrentPage = currentPage,
                Walks = walks
            };
        }

        public IEnumerable<string> AllWalkRegions()
            => this.data
                .Walks
                .Select(x => x.Region.Name)
                .Distinct()
                .OrderBy(r => r)
                .ToList();
        
    }
}
