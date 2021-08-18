using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Linq;
using WalksInNature.Data;
using WalksInNature.Data.Models;
using WalksInNature.Models.Walks;
using WalksInNature.Services.Walks.Models;

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

        public WalkQueryServiceModel All(
                    string region = null, 
                    string searchTerm = null, 
                    WalkSorting sorting = WalkSorting.DateCreated, 
                    int currentPage = 1, 
                    int walksPerPage = int.MaxValue,
                    bool publicOnly = true)
        {
            var walksQuery = this.data.Walks
                .Where(x => !publicOnly || x.IsPublic);

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
                WalkSorting.Likes => walksQuery.OrderByDescending(x => x.Likes.Count),
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
               .Where(x => x.IsPublic)
               .OrderByDescending(x => x.Id)
               .ProjectTo<LatestWalkServiceModel>(this.mapper.ConfigurationProvider)
               .Take(3)
               .ToList();
          
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
                       IsPublic = x.IsPublic,
                       Likes = x.Likes.Count()
                   })
                   .FirstOrDefault();

            return walk;
        }

        public IEnumerable<WalkServiceModel> WalksByUser(string userId)
            => GetWalks(this.data
                .Walks
                .Where(x => x.AddedByUserId == userId));

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
                AddedByUserId = userId,
                IsPublic = false
            };

            this.data.Walks.Add(walkToAdd);
            this.data.SaveChanges();

            return walkToAdd.Id;
        }

        public bool Edit(int id, string name, string imageUrl, string startPoint, int regionId, int levelId, string description, bool isPublic)
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
            walkData.IsPublic = isPublic;

            this.data.SaveChanges();

            return true;
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

        public void ChangeStatus(int walkId)
        {
            var walk = this.data.Walks.Find(walkId);

            walk.IsPublic = !walk.IsPublic;

            this.data.SaveChanges();
        }
         
        public void DeleteByAdmin(int id) 
        {
            var walkToDelete = this.data.Walks.Find(id);

            if (walkToDelete != null)
            {
                this.data.Walks.Remove(walkToDelete);
                this.data.SaveChanges();
            }
        }
                
        public void DeleteByUser(int id, string userId)
        {
            var walkToDelete = this.data.Walks.Find(id);

            if (walkToDelete != null || walkToDelete.AddedByUser.Id == userId)
            {
                this.data.Walks.Remove(walkToDelete);
                this.data.SaveChanges();
            }
        }

        private IEnumerable<WalkServiceModel> GetWalks(IQueryable<Walk> walkQuery)
           => walkQuery
                .ProjectTo<WalkServiceModel>(this.mapper.ConfigurationProvider)               
                .ToList();

        
    }
}
