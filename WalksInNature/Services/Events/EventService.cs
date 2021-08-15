using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WalksInNature.Data;
using WalksInNature.Data.Models;
using WalksInNature.Models.Events;
using WalksInNature.Services.Events.Models;

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

        public EventQueryServiceModel All(
               string guideName = null, 
               string searchTerm = null, 
               EventSorting sorting = EventSorting.DateCreated, 
               int currentPage = 1,
               int eventsPerPage = int.MaxValue,
               bool publicOnly = true)
        {
            var eventsQuery = this.data.Events
                .Where(x => !publicOnly || x.IsPublic);

            if (!string.IsNullOrWhiteSpace(guideName))
            {
                eventsQuery = eventsQuery.Where(x => x.Guide.Name == guideName);
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
                EventSorting.Date => eventsQuery.OrderBy(x => x.Date),
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
               .FirstOrDefault();


        public int Create(string name, string imageUrl, string date, string startHour, 
            string startPoint, int regionId, int levelId, string description, int guideId)
        {
            var eventToAdd = new Event
            {
                Name = name,
                ImageUrl = imageUrl,
                Date = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture),
                StartHour = TimeSpan.ParseExact(startHour, "c", CultureInfo.InvariantCulture),
                StartPoint = startPoint,
                RegionId = regionId,
                LevelId = levelId,
                Description = description,
                GuideId = guideId,
                IsPublic = false
            };

            this.data.Events.Add(eventToAdd);
            this.data.SaveChanges();

            return eventToAdd.Id;
        }

        public bool Edit(int id, string name, string imageUrl, string date, string startHour,
            string startPoint, int regionId, int levelId, string description, bool isPublic)
        {
            var eventData = this.data.Events.Find(id);

            if (eventData == null)
            {
                return false;
            }
            
            eventData.Name = name;
            eventData.ImageUrl = imageUrl;
            eventData.Date = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            eventData.StartHour = TimeSpan.ParseExact(startHour, "c", CultureInfo.InvariantCulture, TimeSpanStyles.None);
            eventData.StartPoint = startPoint;
            eventData.RegionId = regionId;
            eventData.LevelId = levelId;
            eventData.Description = description;
            eventData.IsPublic = isPublic;
           
            this.data.SaveChanges();

            return true;
        }

        public IEnumerable<EventServiceModel> EventsByGuide(string userId)
            => GetEvents(this.data
                .Events
                .Where(e => e.Guide.UserId == userId));
        
        // ToDo
        public IEnumerable<EventServiceModel> EventsByUser(string userId)
           => GetEvents(this.data
               .Events
               .Where(e => e.Guide.UserId == userId));

        public bool EventIsByGuide(int eventId, int guideId)
                => this.data
                    .Events
                    .Any(x => x.Id == eventId && x.GuideId == guideId);

        public void ChangeStatus(int eventId)
        {
            var eventToPublish = this.data.Events.Find(eventId);

            eventToPublish.IsPublic = !eventToPublish.IsPublic;

            this.data.SaveChanges();
        }

        public IEnumerable<string> AllEventGuides()
            => this.data.Events
                .Select(x => x.Guide.Name)
                .Distinct()
                .OrderBy(d => d)
                .ToList();
               
              
       
        public bool AddUserToEvent(string userId, int eventId)
        {
            var userWithEvent = this.data.EventsUsers.Any(x => x.UserId == userId && x.EventId == eventId);
            
            if (userWithEvent)
            {
                return false;
            }

            var eventUser = new EventUser
            {
                EventId = eventId,
                UserId = userId,
            };

            this.data.EventsUsers.Add(eventUser);
            this.data.SaveChanges();

            return true;
        }

        public bool RemoveUserByEvent(string userId, int eventId)
        {
            var userEvent = this.data
                .EventsUsers
                .Where(x => x.UserId == userId && x.EventId == eventId)
                .FirstOrDefault();

            if (userEvent == null)
            {
                throw new ArgumentException(string.Format("User with that ID: {0} does not participate in event with ID: {1}", userId, eventId));
            }

           
            this.data.EventsUsers.Remove(userEvent);
            this.data.SaveChanges();

            return true;
        }
        public void DeleteByAdmin(int id)
        {
            var eventToDelete = this.data.Events.Find(id);

            if (eventToDelete != null)
            {
                this.data.Events.Remove(eventToDelete);
                this.data.SaveChanges();
            }                      

        }
        public void DeleteByGuide(int id, int guideId)
        {
            var eventToDelete = this.data.Events.Find(id);

            if (eventToDelete != null && eventToDelete.GuideId == guideId)
            {
                this.data.Events.Remove(eventToDelete);
                this.data.SaveChanges();
            }
        }

        private IEnumerable<EventServiceModel> GetEvents(IQueryable<Event> eventQuery)
            => eventQuery
                .ProjectTo<EventServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();
    }
}
