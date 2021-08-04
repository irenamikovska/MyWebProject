using AutoMapper;
using WalksInNature.Data.Models;
using WalksInNature.Models.Events;
using WalksInNature.Models.Walks;
using WalksInNature.Services.Events;
using WalksInNature.Services.Walks;

namespace WalksInNature.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Walk, LatestWalkServiceModel>();
            this.CreateMap<WalkDetailsServiceModel, WalkFormModel>();

            this.CreateMap<EventDetailsServiceModel, EventFormModel>();
            this.CreateMap<Event, EventDetailsServiceModel>()
                .ForMember(x => x.UserId, cfg => cfg.MapFrom(x => x.Guide.UserId));
        }

    }
}
