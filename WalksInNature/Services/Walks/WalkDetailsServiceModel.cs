namespace WalksInNature.Services.Walks
{
    public class WalkDetailsServiceModel : WalkServiceModel
    {
        public int RegionId { get; init; }

        public int LevelId { get; init; }

        public string StartPoint { get; set; }

        public string Description { get; set; }        

    }
}
