using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WalksInNature.Data;
using WalksInNature.Data.Models;
using WalksInNature.Infrastructure;
using WalksInNature.Models.Events;

namespace WalksInNature.Controllers
{
    public class EventsController : Controller
    {
        private readonly WalksDbContext data;
        public EventsController(WalksDbContext data) => this.data = data;

        public IActionResult All([FromQuery] AllEventsQueryModel query)
        {
            var eventsQuery = this.data.Events.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Date))
            {
                eventsQuery = eventsQuery.Where(x => x.Date.ToString() == query.Date);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                eventsQuery = eventsQuery.Where(x =>
                    x.Name.ToLower().Contains(query.SearchTerm.ToLower()) ||
                    x.Region.Name.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            eventsQuery = query.Sorting switch
            {
                EventSorting.Name => eventsQuery.OrderBy(x => x.Name),
                EventSorting.Region => eventsQuery.OrderBy(x => x.Region.Id),
                EventSorting.DateCreated or _ => eventsQuery.OrderByDescending(x => x.Id)
            };

            var totalEvents = eventsQuery.Count();

            var events = eventsQuery
                .Skip((query.CurrentPage - 1) * AllEventsQueryModel.EventsPerPage)
                .Take(AllEventsQueryModel.EventsPerPage)
                .Select(x => new EventListingViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Date = x.Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                    StartingHour = x.StartingHour.ToString("hh:mm", CultureInfo.InvariantCulture),
                    ImageUrl = x.ImageUrl,
                    Region = x.Region.Name,
                    Level = x.Level.Name,
                    GuideId = x.GuideId
                })
                .ToList();
            
            var eventDates = this.data
                .Events
                .Select(x => x.Date.ToString())
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            query.TotalEvents = totalEvents;
            query.Dates = eventDates;
            query.Events = events;

            return View(query);
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!this.UserIsGuide())
            {
                return RedirectToAction(nameof(GuidesController.Become), "Guides");
            }

            return View(new AddEventFormModel
            {
                Regions = this.GetEventRegions(),
                Levels = this.GetEventLevels()
            });
        }
         

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddEventFormModel input)
        {
            var guideId = this.data
                .Guides
                .Where(d => d.UserId == this.User.GetId())
                .Select(d => d.Id)
                .FirstOrDefault();

            if (guideId == 0)
            {
                return RedirectToAction(nameof(GuidesController.Become), "Guides");
            }


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
                input.Regions = this.GetEventRegions();
                input.Levels = this.GetEventLevels();
                return View(input);
            }

            var eventToAdd = new Event
            {
                Name = input.Name,
                ImageUrl = input.ImageUrl,
                Date = input.Date,
                StartingHour = input.StartingHour,
                StartPoint = input.StartPoint,
                RegionId = input.RegionId,
                LevelId = input.LevelId,
                Description = input.Description,
                GuideId = guideId
            };

            this.data.Events.Add(eventToAdd);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        private bool UserIsGuide()
            => this.data
                .Guides
                .Any(d => d.UserId == this.User.GetId());

        private IEnumerable<EventRegionViewModel> GetEventRegions()
            => this.data
                .Regions
                .Select(x => new EventRegionViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();

        private IEnumerable<EventLevelViewModel> GetEventLevels()
            => this.data
                .Levels
                .Select(x => new EventLevelViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();
    }
}
