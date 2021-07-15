namespace WalksInNature.Models.Walks
{
    public class WalkListingViewModel
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string ImageUrl { get; init; }
        public string Region { get; set; }
        public string Level { get; set; }
    }
}
