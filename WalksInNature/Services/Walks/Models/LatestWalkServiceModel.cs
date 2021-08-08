namespace WalksInNature.Services.Walks.Models
{
    public class LatestWalkServiceModel : IWalkModel
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string ImageUrl { get; init; }
        public string Region { get; set; }
    }
}
