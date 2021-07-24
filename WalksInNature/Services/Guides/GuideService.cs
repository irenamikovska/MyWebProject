using System.Linq;
using WalksInNature.Data;

namespace WalksInNature.Services.Guides
{
    public class GuideService : IGuideService
    {
        private readonly WalksDbContext data;
        public GuideService(WalksDbContext data)
            => this.data = data;

        public bool IsGuide(string userId)
            => this.data
                .Guides
                .Any(x => x.UserId == userId);

        public int GetGuideId(string userId)
            => this.data
                .Guides
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                .FirstOrDefault();

    }
}
