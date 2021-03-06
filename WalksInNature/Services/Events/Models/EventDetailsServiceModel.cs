namespace WalksInNature.Services.Events.Models
{
    public class EventDetailsServiceModel : EventServiceModel
    {
        public string StartPoint { get; init; }

        public int RegionId { get; init; }

        public int LevelId { get; init; }

        public string Description { get; init; }

        public string GuidePhoneNumber { get; set; }

        public string UserId { get; set; }

    }
}

