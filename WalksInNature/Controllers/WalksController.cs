using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalksInNature.Infrastructure;
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
        private readonly IMapper mapper;
        public WalksController(
            IWalkService walkService, 
            IRegionService regionService, 
            ILevelService levelService,
            IMapper mapper)
        {
            this.walkService = walkService;
            this.regionService = regionService;
            this.levelService = levelService;
            this.mapper = mapper;
        }

        public IActionResult All([FromQuery] AllWalksQueryModel query)
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
        public IActionResult MyWalks()
        {
            var myWalks = this.walkService.WalksByUser(this.User.GetId());

            return View(myWalks);
        }
       
        [Authorize]
        public IActionResult Details(int id, string information)
        {
            var walk = this.walkService.GetDetails(id);

            if (information != walk.GetWalkInformation())
            {
                return BadRequest();
            }

            return this.View(walk);
        }
        
        [Authorize]
        public IActionResult Add() => View(new WalkFormModel 
        {
            Regions = this.regionService.GetRegions(),
            Levels = this.levelService.GetLevels()
            
        });

        [HttpPost]
        [Authorize]
        public IActionResult Add(WalkFormModel input)
        {
            var userId = this.User.GetId();

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
             
            var walkId = this.walkService.Create(       
                input.Name,
                input.ImageUrl,
                input.StartPoint,
                input.RegionId,
                input.LevelId,
                input.Description,
                userId
            );
                    
            return RedirectToAction(nameof(Details), new { id = walkId, information = input.GetWalkInformation() });
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.GetId();            

            var walkToEdit = this.walkService.GetDetails(id);

            if (walkToEdit.UserId != userId && !User.IsAdmin())
            {
                return Unauthorized();
            }
                     
            var walkForm = this.mapper.Map<WalkFormModel>(walkToEdit);

            walkForm.Regions = this.regionService.GetRegions();
            walkForm.Levels = this.levelService.GetLevels();

            return View(walkForm);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, WalkFormModel walk)
        {
            var userId = this.User.GetId();

            if (!this.regionService.RegionExists(walk.RegionId))
            {
                this.ModelState.AddModelError(nameof(walk.RegionId), "Region does not exist.");
            }

            if (!this.levelService.LevelExists(walk.LevelId))
            {
                this.ModelState.AddModelError(nameof(walk.LevelId), "Level does not exist.");
            }

            if (!ModelState.IsValid)
            {
                walk.Regions = this.regionService.GetRegions();
                walk.Levels = this.levelService.GetLevels();

                return View(walk);
            }

            var walkToEdit = this.walkService.GetDetails(id);         


            if (walkToEdit.UserId != userId && !User.IsAdmin())
            {
                return BadRequest();
            }

            var editedWalk = this.walkService.Edit(
                id,
                walk.Name,
                walk.ImageUrl,
                walk.StartPoint,
                walk.RegionId,
                walk.LevelId,
                walk.Description,
                this.User.IsAdmin());

            if (!editedWalk)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Details), new { id, information = walk.GetWalkInformation() });
        }

        [Authorize]     
        public IActionResult AddLike(int id)
        {
            
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var userId = this.User.GetId();

            this.walkService.AddUserToWalk(userId, id);         
            
            return RedirectToAction(nameof(All));
            //return this.RedirectToAction(nameof(this.Details), new { id });
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId) && !User.IsAdmin())
            {
                return this.BadRequest();
            }

            this.walkService.Delete(id, userId);

            return RedirectToAction(nameof(All));            
        }
    }
}
