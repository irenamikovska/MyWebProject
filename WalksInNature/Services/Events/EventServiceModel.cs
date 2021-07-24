using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalksInNature.Services.Events
{
    public class EventServiceModel
    {
        public int Id { get; init; }
        public string Date { get; init; }
        public string StartingHour { get; init; }
        public string Name { get; init; }
        public string ImageUrl { get; init; }
        public string Region { get; set; }
        public string Level { get; set; }
        public int GuideId { get; set; }
    }
}

