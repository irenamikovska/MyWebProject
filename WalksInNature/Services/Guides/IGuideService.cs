namespace WalksInNature.Services.Guides
{
    public interface IGuideService
    {
        public bool IsGuide(string userId);
        public int GetGuideId(string userId);
        
    }
}
