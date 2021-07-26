using System.Linq;
using WalksInNature.Data;
using WalksInNature.Data.Models;

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

        public int Create(string name, string phoneNumber, string userId) 
        {
            var guideToAdd = new Guide
            {
                Name = name,
                PhoneNumber = phoneNumber,
                UserId = userId
            };

            this.data.Guides.Add(guideToAdd);
            this.data.SaveChanges();

            return guideToAdd.Id;
        }
    }
}
