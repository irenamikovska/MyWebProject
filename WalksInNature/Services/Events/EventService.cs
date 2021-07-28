using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WalksInNature.Data;
using WalksInNature.Data.Models;
using WalksInNature.Models.Events;

namespace WalksInNature.Services.Events
{
    public class EventService : IEventService
    {
        private readonly WalksDbContext data;
        private readonly IMapper mapper;
        public EventService(WalksDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }
        

        public EventQueryServiceModel All(string date, string searchTerm, 
            EventSorting sorting, int currentPage, int eventsPerPage)
        {
            var eventsQuery = this.data.Events.AsQueryable();

            if (!string.IsNullOrWhiteSpace(date))
            {
                eventsQuery = eventsQuery.Where(x => x.Date.ToString() == date);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                eventsQuery = eventsQuery.Where(x =>
                    x.Name.ToLower().Contains(searchTerm.ToLower()) ||
                    x.Region.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            eventsQuery = sorting switch
            {
                EventSorting.Name => eventsQuery.OrderBy(x => x.Name),
                EventSorting.Region => eventsQuery.OrderBy(x => x.Region.Id),
                EventSorting.DateCreated or _ => eventsQuery.OrderByDescending(x => x.Id)
            };

            var totalEvents = eventsQuery.Count();

            var events = GetEvents(eventsQuery
                .Skip((currentPage - 1) * eventsPerPage)
                .Take(eventsPerPage));                

            return new EventQueryServiceModel
            {
                TotalEvents = totalEvents,
                CurrentPage = currentPage,
                EventsPerPage = eventsPerPage,
                Events = events
            };

        }        

        public EventDetailsServiceModel GetDetails(int id)
           => this.data
               .Events
               .Where(x => x.Id == id)
               .ProjectTo<EventDetailsServiceModel>(this.mapper.ConfigurationProvider)
               /*
                .Select(x => new EventDetailsServiceModel
               {
                   Id = x.Id,
                   Name = x.Name,
                   ImageUrl = x.ImageUrl,
                   StartPoint = x.StartPoint,
                   Region = x.Region.Name,
                   Level = x.Level.Name,
                   Date = x.Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                   StartingHour = x.StartingHour.ToString("hh:mm", CultureInfo.InvariantCulture),
                   Description = x.Description,
                   GuideId = x.GuideId,                   
                   UserId = x.Guide.UserId
               })*/
               .FirstOrDefault();


        public int Create(string name, string imageUrl, string date, string startingHour, 
            string startPoint, int regionId, int levelId, string description, int guideId)
        {
            var eventToAdd = new Event
            {
                Name = name,
                ImageUrl = imageUrl,
                Date = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture),
                StartingHour = DateTime.ParseExact(startingHour, "hh:mm", CultureInfo.InvariantCulture),
                StartPoint = startPoint,
                RegionId = regionId,
                LevelId = levelId,
                Description = description,
                GuideId = guideId
            };

            this.data.Events.Add(eventToAdd);
            this.data.SaveChanges();

            return eventToAdd.Id;
        }

        public bool Edit(int id, string name, string imageUrl, string date, string startingHour,
            string startPoint, int regionId, int levelId, string description)
        {
            var eventData = this.data.Events.Find(id);

            if (eventData == null)
            {
                return false;
            }
            
            eventData.Name = name;
            eventData.ImageUrl = imageUrl;
            eventData.Date = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            eventData.StartingHour = DateTime.ParseExact(startingHour, "hh:mm", CultureInfo.InvariantCulture);
            eventData.StartPoint = startPoint;
            eventData.RegionId = regionId;
            eventData.LevelId = levelId;
            eventData.Description = description;
           
            this.data.SaveChanges();

            return true;
        }

        public IEnumerable<EventServiceModel> EventsByGuide(string userId)
            => GetEvents(this.data
                .Events
                .Where(e => e.Guide.UserId == userId));

        public bool EventIsByGuide(int eventId, int guideId)
                => this.data
                    .Events
                    .Any(x => x.Id == eventId && x.GuideId == guideId);

        public IEnumerable<string> AllEventDates()
            => this.data.Events
                .Select(x => x.Date.ToString())
                .Distinct()
                .OrderBy(d => d)
                .ToList();
               

        private static IEnumerable<EventServiceModel> GetEvents(IQueryable<Event> eventQuery)
            => eventQuery
                .Select(x => new EventServiceModel
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

    }
}
