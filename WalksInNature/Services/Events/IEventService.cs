using System.Collections.Generic;
using WalksInNature.Models.Events;
using WalksInNature.Services.Events.Models;

namespace WalksInNature.Services.Events
{
    public interface IEventService
    {
        EventQueryServiceModel All(string date = null,
               string searchTerm = null,
               EventSorting sorting = EventSorting.DateCreated,
               int currentPage = 1,
               int eventsPerPage = int.MaxValue,
               bool publicOnly = true);
        IEnumerable<string> AllEventDates();

        EventDetailsServiceModel GetDetails(int eventId);

        IEnumerable<EventServiceModel> EventsByGuide(string userId);

        IEnumerable<EventServiceModel> EventsByUser(string userId);

        bool EventIsByGuide(int eventId, int guideId);
        
        int Create(string name, string imageUrl, string date, string startingHour,
            string startPoint, int regionId, int levelId, string description, int guideId);

        bool Edit(int id, string name, string imageUrl, string date, string startingHour,
            string startPoint, int regionId, int levelId, string description, bool isPublic);

        void ChangeStatus(int eventId);
        bool AddUserToEvent(string userId, int eventId);
        bool RemoveUserByEvent(string userId, int eventId);
        void Delete(int id, int guideId);

    }
}
