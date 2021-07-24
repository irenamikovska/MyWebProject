using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalksInNature.Services.Events
{
    public class EventQueryServiceModel
    {
        public int CurrentPage { get; init; }
        public int EventsPerPage { get; init; }
        public int TotalEvents { get; init; }
        public IEnumerable<EventServiceModel> Events { get; init; }

    }
}
