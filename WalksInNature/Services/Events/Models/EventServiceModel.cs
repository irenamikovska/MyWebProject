using System;
using System.Collections.Generic;


namespace WalksInNature.Services.Events.Models
{
    public class EventServiceModel : IEventModel
    {
        public int Id { get; init; }
        public DateTime Date { get; init; }
        public string DateFormated => this.Date.Date.ToString("dd.MM.yyyy");
        public TimeSpan StartHour { get; init; }
        public string HourFormated => this.StartHour.ToString(@"hh\:mm");
        public string Name { get; init; }
        public string ImageUrl { get; init; }
        public string Region { get; set; }
        public string Level { get; set; }
        public int GuideId { get; set; }
        public string GuideName { get; set; }
        public int Participants { get; set; }
        public bool IsPublic { get; init; }
       
    }
}

