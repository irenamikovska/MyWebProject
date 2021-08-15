using System.Collections.Generic;

namespace WalksInNature.Services.Events.Models
{
    public class EventQueryServiceModel
    {
        public int CurrentPage { get; init; }
        public int EventsPerPage { get; init; }
        public int TotalEvents { get; init; }
        public IEnumerable<EventServiceModel> Events { get; init; }

    }
}
