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

            return RedirectToAction("Index", "Home");
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
