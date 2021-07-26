namespace WalksInNature.Services.Guides
{
    public interface IGuideService
    {
        bool IsGuide(string userId);
        int GetGuideId(string userId);
        int Create(string name, string phoneNumber, string userId);

    }
}
