﻿using System.Collections.Generic;
using WalksInNature.Models.Events;

namespace WalksInNature.Services.Events
{
    public interface IEventService
    {
        EventQueryServiceModel All(string date, string searchTerm, EventSorting sorting, int currentPage, int eventsPerPage);
        IEnumerable<string> AllEventDates();

        EventDetailsServiceModel GetDetails(int eventId);

        IEnumerable<EventServiceModel> ByUser(string userId);

        bool IsByGuide(int eventId, int guideId);

        int Create(string name, string imageUrl, string date, string startingHour,
            string startPoint, int regionId, int levelId, string description, int guideId);

        bool Edit(int id, string name, string imageUrl, string date, string startingHour,
            string startPoint, int regionId, int levelId, string description);
    }
}