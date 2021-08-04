namespace WalksInNature.Services.Walks
{
    public class LatestWalkServiceModel
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string ImageUrl { get; init; }
        public string Region { get; set; }
    }
}
