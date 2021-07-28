using System.Collections.Generic;
using System.Linq;
using WalksInNature.Data;
using WalksInNature.Data.Models;
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

            var walks = GetWalks(walksQuery
                .Skip((currentPage - 1) * walksPerPage)
                .Take(walksPerPage));
                

            return new WalkQueryServiceModel
            {
                TotalWalks = totalWalks,
                WalksPerPage = walksPerPage,
                CurrentPage = currentPage,
                Walks = walks
            };
        }

        public int Create(string name, string imageUrl, string startPoint, 
            int regionId, int levelId, string description, string userId)
        {
            var walkToAdd = new Walk
            {
                Name = name,
                ImageUrl = imageUrl,
                StartPoint = startPoint,
                RegionId = regionId,
                LevelId = levelId,
                Description = description,
                UserId = userId
            };

            this.data.Walks.Add(walkToAdd);
            this.data.SaveChanges();

            return walkToAdd.Id;
        }

        public IEnumerable<string> AllWalkRegions()
            => this.data
                .Walks
                .Select(x => x.Region.Name)
                .Distinct()
                .OrderBy(r => r)
                .ToList();

        public WalkDetailsServiceModel GetDetails(int walkId)        
             => this.data
               .Walks
               .Where(x => x.Id == walkId)
               .Select(x => new WalkDetailsServiceModel
               {
                   Id = x.Id,
                   Name = x.Name,
                   ImageUrl = x.ImageUrl,
                   StartPoint = x.StartPoint,
                   Region = x.Region.Name,
                   Level = x.Level.Name,                  
                   Description = x.Description,                
                   UserId = x.UserId
               })
               .FirstOrDefault();

        public IEnumerable<WalkServiceModel> WalksByUser(string userId)
            => GetWalks(this.data
                .Walks
                .Where(x => x.UserId == userId));       

        public bool Edit(int id, string name, string imageUrl, string startPoint, int regionId, int levelId, string description)
        {
            var walkData = this.data.Walks.Find(id);

            if (walkData == null)
            {
                return false;
            }

            walkData.Name = name;
            walkData.ImageUrl = imageUrl;           
            walkData.StartPoint = startPoint;
            walkData.RegionId = regionId;
            walkData.LevelId = levelId;
            walkData.Description = description;

            this.data.SaveChanges();

            return true;
        }

        private static IEnumerable<WalkServiceModel> GetWalks(IQueryable<Walk> walkQuery)
           => walkQuery
               .Select(x => new WalkServiceModel
               {
                   Id = x.Id,
                   Name = x.Name,                   
                   ImageUrl = x.ImageUrl,
                   Region = x.Region.Name,
                   Level = x.Level.Name,
                   UserId = x.UserId
               })
               .ToList();

        
    }
}
