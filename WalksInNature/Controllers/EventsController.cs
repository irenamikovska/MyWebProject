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
                query.GuideName,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllEventsQueryModel.EventsPerPage);

            var eventGuides = this.eventService.AllEventGuides();

            query.Guides = eventGuides;
            query.TotalEvents = queryResult.TotalEvents;
            query.Events = queryResult.Events;

            return View(query);
        }

        [Authorize]
        public IActionResult MyGuideEvents()
        {
            var userId = this.User.GetId();

            var myEvents = this.eventService.EventsByGuide(userId);

            return View(myEvents);
        }

        [Authorize]
        public IActionResult MyUserEvents()
        {
            var userId = this.User.GetId();           

            var myEvents = this.eventService.EventsByUser(userId);

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

            if (Convert.ToByte(input.StartHour.Substring(0, 2)) > 23 && Convert.ToByte(input.StartHour.Substring(3, 2)) > 59)
            {
                this.ModelState.AddModelError(nameof(input.StartHour), "Hour have to be between 00:00 and 23:59.");
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
                    input.StartHour,
                    input.StartPoint,
                    input.RegionId,
                    input.LevelId,
                    input.Description,
                    guideId);

            TempData[GlobalMessageKey] = "Your event was added and is awaiting for approval!";

            return RedirectToAction(nameof(Details), new { id = eventId, information = input.GetEventInformation() });

        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.GetId();

            var guideId = this.guideService.GetGuideId(userId);

            if (!this.guideService.IsGuide(userId) && !User.IsAdmin())
            {
                TempData[GlobalMessageKey] = "You have to be a guide in order to edit an event!";

                return RedirectToAction(nameof(GuidesController.Become), "Guides");
            }

            var eventToEdit = this.eventService.GetDetails(id);

            if (eventToEdit.GuideId != guideId && !User.IsAdmin())
            {
                TempData[GlobalMessageKey] = "You have not permission to edit this event!";

                return RedirectToAction(nameof(Details), new { id = id, information = eventToEdit.GetEventInformation() });
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
                TempData[GlobalMessageKey] = "You have to be a guide in order to edit an event!";
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
                TempData[GlobalMessageKey] = "You have not permission to edit this event!";

                return RedirectToAction(nameof(Details), new { id = id, information = eventToEdit.GetEventInformation() });
            }

            var editedEvent = this.eventService.Edit(
                id,
                eventToEdit.Name,
                eventToEdit.ImageUrl,
                eventToEdit.Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                eventToEdit.StartHour,
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

            TempData[GlobalMessageKey] = $"The event was edited{(this.User.IsAdmin() ? string.Empty : " and is awaiting for approval")}!";

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

            var guideId = this.guideService.GetGuideId(userId);

            var eventToDelete = this.eventService.GetDetails(id);

            if (!this.eventService.EventIsByGuide(id, guideId) && !User.IsAdmin())
            {
                TempData[GlobalMessageKey] = $"You have not permission to delete this event!";

                return RedirectToAction(nameof(Details), new { id, information = eventToDelete.GetEventInformation() });
            }                      

            if (eventToDelete.IsPublic == true)
            {
                TempData[GlobalMessageKey] = $"Your event can not be deleted as it is with status Approved. Please contact admin!";

                return RedirectToAction(nameof(Details), new { id, information = eventToDelete.GetEventInformation() });
            }

            this.eventService.DeleteByGuide(id, guideId);

            return RedirectToAction(nameof(All));
        }

    }
}
