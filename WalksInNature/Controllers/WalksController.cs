using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalksInNature.Models.Walks;
using WalksInNature.Services.Levels;
using WalksInNature.Services.Regions;
using WalksInNature.Services.Walks;

namespace WalksInNature.Controllers
{
    public class WalksController : Controller
    {
        private readonly IWalkService walkService;
        private readonly IRegionService regionService;
        private readonly ILevelService levelService;
        
        public WalksController(IWalkService walkService,
            IRegionService regionService,
            ILevelService levelService) 
        {
            this.walkService = walkService;
            this.regionService = regionService;
            this.levelService = levelService;            
        } 
               
        public IActionResult All([FromQuery]AllWalksQueryModel query)
        {
            var queryResult = this.walkService.All(
                query.Region,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllWalksQueryModel.WalksPerPage);


            var walkRegions = this.walkService.AllWalkRegions();

            query.Regions = walkRegions;
            query.TotalWalks = queryResult.TotalWalks;
            query.Walks = queryResult.Walks;

            return View(query);            
        }

        [Authorize]
        public IActionResult Add() => View(new AddWalkFormModel 
        {
            Regions = this.regionService.GetRegions(),
            Levels = this.levelService.GetLevels()
            
        });

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddWalkFormModel input)
        {
            if (!this.regionService.RegionExists(input.RegionId))
            {
                this.ModelState.AddModelError(nameof(input.RegionId), "Region does not exist.");
            }

            if (!this.levelService.LevelExists(input.LevelId))
            {
                this.ModelState.AddModelError(nameof(input.LevelId), "Level does not exist.");
            }


            if (!ModelState.IsValid)
            {
                input.Regions = this.regionService.GetRegions();
                input.Levels = this.levelService.GetLevels();

                return View(input);
            }
             
            this.walkService.Create(       
                input.Name,
                input.ImageUrl,
                input.StartPoint,
                input.RegionId,
                input.LevelId,
                input.Description
            );
                    
            return RedirectToAction(nameof(All));
        }
                

    }
}
