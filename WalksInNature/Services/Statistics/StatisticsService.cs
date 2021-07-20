using System.Linq;
using WalksInNature.Data;

namespace WalksInNature.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly WalksDbContext data;
        public StatisticsService(WalksDbContext data)
            => this.data = data;

        public StatisticsServiceModel Total()
        {
            var totalWalks = this.data.Walks.Count();
            var totalUsers = this.data.Users.Count();
            var totalEvents = this.data.Events.Count();

            return new StatisticsServiceModel
            {
                TotalWalks = totalWalks,
                TotalUsers = totalUsers,
                TotalEvents = totalEvents
            };
        }
    }
}
