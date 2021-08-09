using AutoMapper;
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

using static WalksInNature.WebConstants;

namespace WalksInNature.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventService eventService;
        private readonly IGuideService guideService;
        private readonly IRegionService regionService;
        private readonly ILevelService levelService;
        private readonly IMapper mapper;
        public EventsController(
            IEventService eventService,
            IGuideService guideService,
            IRegionService regionService,
            ILevelService levelService,
            IMapper mapper)
        {
            this.eventService = eventService;
            this.guideService = guideService;
            this.regionService = regionService;
            this.levelService = levelService;
            this.mapper = mapper;
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
        public IActionResult MyUserEvents()
        {
            var myEvents = this.eventService.EventsByUser(this.User.GetId());

            return View(myEvents);
        }

        [Authorize]
        public IActionResult Details(int id, string information)
        {
            var eventDetails = this.eventService.GetDetails(id);

            if (information != eventDetails.GetEventInformation())
            {
                return BadRequest();
            }

            return this.View(eventDetails);
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

            var eventId = this.eventService.Create(
                    input.Name,
                    input.ImageUrl,
                    input.Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                    input.StartingHour.ToString("hh:mm", CultureInfo.InvariantCulture),
                    input.StartPoint,
                    input.RegionId,
                    input.LevelId,
                    input.Description,
                    guideId);

            TempData[GlobalMessageKey] = "You event was added and is awaiting for approval!";

            return RedirectToAction(nameof(Details), new { id = eventId, information = input.GetEventInformation() });

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
                       
            var eventForm = this.mapper.Map<EventFormModel>(eventToEdit);

            eventForm.Regions = this.regionService.GetRegions();
            eventForm.Levels = this.levelService.GetLevels();

            return View(eventForm);

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

            var editedEvent = this.eventService.Edit(
                id,
                eventToEdit.Name,
                eventToEdit.ImageUrl,
                eventToEdit.Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                eventToEdit.StartingHour.ToString("hh:mm", CultureInfo.InvariantCulture),
                eventToEdit.StartPoint,
                eventToEdit.RegionId,
                eventToEdit.LevelId,
                eventToEdit.Description,
                this.User.IsAdmin()
               );

            if (!editedEvent)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = $"You event was edited{(this.User.IsAdmin() ? string.Empty : " and is awaiting for approval")}!";

            return RedirectToAction(nameof(Details), new { id, information = eventToEdit.GetEventInformation() });
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

            TempData[GlobalMessageKey] = $"You joined an event successfully!";
            
            return RedirectToAction(nameof(All));
                      
        }

        [Authorize]
        public IActionResult RemoveUser(int id)
        {
            var userId = this.User.GetId();
            
            this.eventService.RemoveUserByEvent(userId, id);

            TempData[GlobalMessageKey] = $"You left an event successfully!";

            return RedirectToAction(nameof(All));           
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var userId = this.User.GetId();

            var guideId = this.guideService.GetGuideId(this.User.GetId());

            if (guideId == 0 && !User.IsAdmin())
            {
                return this.BadRequest();                
            }

            this.eventService.Delete(id, guideId);

            return RedirectToAction(nameof(All));
        }


    }
}
