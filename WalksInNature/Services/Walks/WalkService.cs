using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Configuration;
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
        private readonly IMapper mapper;
        public WalkService(WalksDbContext data, IMapper mapper)
        { 
            this.data = data;
            this.mapper = mapper;
        }
        

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

        public IEnumerable<LatestWalkServiceModel> Latest()
           => this.data
               .Walks
               .OrderByDescending(c => c.Id)
               .ProjectTo<LatestWalkServiceModel>(this.mapper.ConfigurationProvider)
               .Take(3)
               .ToList();

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
                AddedByUserId = userId
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
        {
            var walk = this.data
                   .Walks
                   .Where(x => x.Id == walkId)
                   .Select(x => new WalkDetailsServiceModel
                   {
                       Id = x.Id,
                       Name = x.Name,
                       ImageUrl = x.ImageUrl,
                       StartPoint = x.StartPoint,
                       RegionId = x.RegionId,
                       Region = x.Region.Name,
                       LevelId = x.LevelId,
                       Level = x.Level.Name,
                       Description = x.Description,
                       UserId = x.AddedByUserId,
                       Likes = x.Likes.Count()
                   })
                   .FirstOrDefault();

            return walk;
        }
        public bool AddUserToWalk(string userId, int walkId)
        {
            var userWithLike = this.data.WalksUsers.Any(x => x.UserId == userId && x.WalkId == walkId);

            if (userWithLike)
            {
                return false;
            }
            
            var like = new WalkUser
            {
                WalkId = walkId,
                UserId = userId,
            };

            this.data.WalksUsers.Add(like);
            this.data.SaveChanges();

            return true;
        }

        public IEnumerable<WalkServiceModel> WalksByUser(string userId)
            => GetWalks(this.data
                .Walks
                .Where(x => x.AddedByUserId == userId));       

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

        public void Delete(int id, string userId) 
        {
            var walkToDelete = this.data.Walks.Find(id);

            if (walkToDelete.AddedByUserId == userId)
            {
                this.data.Walks.Remove(walkToDelete);
                this.data.SaveChanges();
            }

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
                   UserId = x.AddedByUserId,
                   Likes = x.Likes.Count()
               })
               .ToList();

        
    }
}
