using WalksInNature.Services.Events.Models;
using WalksInNature.Services.Walks.Models;

namespace WalksInNature.Infrastructure
{
    public static class ModelExtensions
    {
        public static string GetEventInformation(this IEventModel eventModel)
            => eventModel.Name;

        public static string GetWalkInformation(this IWalkModel walkModel)
            => walkModel.Name;

    }
}
