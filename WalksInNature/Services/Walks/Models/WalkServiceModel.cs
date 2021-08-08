namespace WalksInNature.Services.Walks.Models
{
    public class WalkServiceModel : IWalkModel
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string ImageUrl { get; init; }
        public string Region { get; set; }
        public string Level { get; set; }
        public string UserId { get; set; }
        public int Likes { get; set; }
        public bool IsPublic { get; init; }

    }
}

