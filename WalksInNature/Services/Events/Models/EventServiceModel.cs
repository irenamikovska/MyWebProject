namespace WalksInNature.Services.Events.Models
{
    public class EventServiceModel : IEventModel
    {
        public int Id { get; init; }
        public string Date { get; init; }
        public string StartingHour { get; init; }
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

