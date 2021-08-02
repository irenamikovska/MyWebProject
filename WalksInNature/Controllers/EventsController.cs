using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using WalksInNature.Infrastructure;
using WalksInNature.Models.Events;
using WalksInNature.Services.Events;
using WalksInNature.Services.Guides;
using WalksInNature.Services.Levels;
using WalksInNature.Services.Regions;

namespace WalksInNature.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventService eventService;
        private readonly IGuideService guideService;
        private readonly IRegionService regionService;
        private readonly ILevelService levelService;
        public EventsController(
            IEventService eventService,
            IGuideService guideService,
            IRegionService regionService,
            ILevelService levelService)
        {
            this.eventService = eventService;
            this.guideService = guideService;
            this.regionService = regionService;
            this.levelService = levelService;
        }

        public IActionResult All([FromQuery] AllEventsQueryModel query)
        {
            var queryResult = this.eventService.All(
                query.Date,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllEventsQueryModel.EventsPerPage);

            var eventDates = this.eventService.AllEventDates();

            query.Dates = eventDates;
            query.TotalEvents = queryResult.TotalEvents;
            query.Events = queryResult.Events;

            return View(query);
        }

        [Authorize]
        public IActionResult MyEvents()
        {
            var myEvents = this.eventService.EventsByGuide(this.User.GetId());

            return View(myEvents);
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!this.guideService.IsGuide(this.User.GetId()))
            {
                return RedirectToAction(nameof(GuidesController.Become), "Guides");
            }

            return View(new EventFormModel
            {
                Regions = this.regionService.GetRegions(),
                Levels = this.levelService.GetLevels()
            });
        }


        [HttpPost]
        [Authorize]
        public IActionResult Add(EventFormModel input)
        {
            var guideId = this.guideService.GetGuideId(this.User.GetId());

            if (guideId == 0)
            {
                return RedirectToAction(nameof(GuidesController.Become), "Guides");
            }

            if (input.Date < DateTime.UtcNow)
            {
                this.ModelState.AddModelError(nameof(input.Date), "Date have to be after current date.");
            }


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

            this.eventService.Create(input.Name, input.ImageUrl,
                input.Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                input.StartingHour.ToString("hh:mm", CultureInfo.InvariantCulture),
                input.StartPoint, input.RegionId, input.LevelId,
                input.Description, guideId);

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.GetId();

            if (!this.guideService.IsGuide(userId) && !User.IsAdmin())
            {
                return RedirectToAction(nameof(GuidesController.Become), "Guides");
            }

            var eventToEdit = this.eventService.GetDetails(id);

            if (eventToEdit.UserId != userId && !User.IsAdmin())
            {
                return Unauthorized();
            }

            return View(new EventFormModel
            {
                Name = eventToEdit.Name,
                ImageUrl = eventToEdit.ImageUrl,
                StartPoint = eventToEdit.StartPoint,
                RegionId = eventToEdit.RegionId,
                LevelId = eventToEdit.LevelId,
                Date = DateTime.ParseExact(eventToEdit.Date, "dd.MM.yyyy", CultureInfo.InvariantCulture),
                StartingHour = DateTime.ParseExact(eventToEdit.StartingHour, "hh:mm", CultureInfo.InvariantCulture),
                Description = eventToEdit.Description,
                GuideId = eventToEdit.GuideId,
                Regions = this.regionService.GetRegions(),
                Levels = this.levelService.GetLevels()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, EventFormModel eventToEdit)
        {
            var guideId = this.guideService.GetGuideId(this.User.GetId());

            if (guideId == 0 && !User.IsAdmin())
            {
                return RedirectToAction(nameof(GuidesController.Become), "Guides");
            }

            if (!this.regionService.RegionExists(eventToEdit.RegionId))
            {
                this.ModelState.AddModelError(nameof(eventToEdit.RegionId), "Region does not exist.");
            }

            if (!this.levelService.LevelExists(eventToEdit.LevelId))
            {
                this.ModelState.AddModelError(nameof(eventToEdit.LevelId), "Level does not exist.");
            }

            if (!ModelState.IsValid)
            {
                eventToEdit.Regions = this.regionService.GetRegions();
                eventToEdit.Levels = this.levelService.GetLevels();

                return View(eventToEdit);
            }

            if (!this.eventService.EventIsByGuide(id, guideId) && !User.IsAdmin())
            {
                return BadRequest();
            }

            this.eventService.Edit(
                id,
                eventToEdit.Name,
                eventToEdit.ImageUrl,
                eventToEdit.Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                eventToEdit.StartingHour.ToString("hh:mm", CultureInfo.InvariantCulture),
                eventToEdit.StartPoint,
                eventToEdit.RegionId,
                eventToEdit.LevelId,
                eventToEdit.Description
               );

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult AddUser(int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var user = this.User.GetId();

            this.eventService.AddUserToEvent(user, id);

            return this.RedirectToAction(nameof(All));
            //return this.RedirectToAction(nameof(this.Details), new { id });
        }

    }
}
