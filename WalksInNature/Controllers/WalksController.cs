using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WalksInNature.Data;
using WalksInNature.Data.Models;
using WalksInNature.Models.Walks;

namespace WalksInNature.Controllers
{
    public class WalksController : Controller
    {
        private readonly WalksDbContext data;
        public WalksController(WalksDbContext data) => this.data = data;

        // bind to model class properties
        // public IActionResult All(string searchTerm, string region, WalkSorting sorting, int currentPage)
        public IActionResult All([FromQuery]AllWalksQueryModel query)
        {
            var walksQuery = this.data.Walks.AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(query.Region))
            {
                walksQuery = walksQuery.Where(x => x.Region.Name == query.Region);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                walksQuery = walksQuery.Where(x =>
                    x.Name.ToLower().Contains(query.SearchTerm.ToLower()) ||
                    x.Region.Name.ToLower().Contains(query.SearchTerm.ToLower()));
            }
            
            walksQuery = query.Sorting switch
            {
                WalkSorting.Name => walksQuery.OrderBy(x => x.Name),
                WalkSorting.Level => walksQuery.OrderByDescending(x => x.Level.Name),
                WalkSorting.DateCreated or _ => walksQuery.OrderByDescending(x => x.Id)
            };
            
            var totalWalks = walksQuery.Count();

            var walks = walksQuery  
                .Skip((query.CurrentPage - 1) * AllWalksQueryModel.WalksPerPage)
                .Take(AllWalksQueryModel.WalksPerPage)               
                .Select(x => new WalkListingViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ImageUrl = x.ImageUrl,
                    Region = x.Region.Name,
                    Level = x.Level.Name
                })
                .ToList();

            var walkRegions = this.data
                .Walks
                .Select(x => x.Region.Name)
                .Distinct()
                .OrderBy(r => r)
                .ToList();

            query.TotalWalks = totalWalks;
            query.Regions = walkRegions;
            query.Walks = walks;

            return View(query);            
        }

        public IActionResult Add() => View(new AddWalkFormModel 
        {
            Regions = this.GetWalkRegions(),
            Levels = this.GetWalkLevels()
        });

        [HttpPost]
        public IActionResult Add(AddWalkFormModel input)
        {
            if (!this.data.Regions.Any(x => x.Id == input.RegionId)) 
            {
                this.ModelState.AddModelError(nameof(input.RegionId), "Region does not exist.");
            }

            if (!this.data.Levels.Any(x => x.Id == input.LevelId))
            {
                this.ModelState.AddModelError(nameof(input.LevelId), "Level does not exist.");
            }

            if (!ModelState.IsValid)
            {
                input.Regions = this.GetWalkRegions();
                input.Levels = this.GetWalkLevels();
                return View(input);
            }

            var walkToAdd = new Walk
            {
                Name = input.Name,
                ImageUrl = input.ImageUrl,
                StartPoint = input.StartPoint,
                RegionId = input.RegionId,
                LevelId = input.LevelId,
                Description = input.Description
            };

            this.data.Walks.Add(walkToAdd);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        private IEnumerable<WalkRegionViewModel> GetWalkRegions()
            => this.data
                .Regions
                .Select(x => new WalkRegionViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();

        private IEnumerable<WalkLevelViewModel> GetWalkLevels()
            => this.data
                .Levels
                .Select(x => new WalkLevelViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();

    }
}
