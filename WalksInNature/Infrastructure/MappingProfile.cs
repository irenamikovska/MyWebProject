using AutoMapper;
using WalksInNature.Data.Models;
using WalksInNature.Models.Events;
using WalksInNature.Models.Walks;
using WalksInNature.Services.Events.Models;
using WalksInNature.Services.Levels;
using WalksInNature.Services.Regions;
using WalksInNature.Services.Walks.Models;

namespace WalksInNature.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Region, RegionServiceModel>();

            this.CreateMap<Level, LevelServiceModel>();

            this.CreateMap<Walk, LatestWalkServiceModel>();

            this.CreateMap<WalkDetailsServiceModel, WalkFormModel>();

            this.CreateMap<Walk, WalkServiceModel>()
                .ForMember(x => x.Region, cfg => cfg.MapFrom(x => x.Region.Name))
                .ForMember(x => x.Level, cfg => cfg.MapFrom(x => x.Level.Name))
                .ForMember(x => x.UserId, cfg => cfg.MapFrom(x => x.AddedByUserId))
                .ForMember(x => x.Likes, cfg => cfg.MapFrom(x => x.Likes.Count));

            this.CreateMap<EventDetailsServiceModel, EventFormModel>();

            this.CreateMap<Event, EventDetailsServiceModel>()
                .ForMember(x => x.UserId, cfg => cfg.MapFrom(x => x.Guide.UserId))
                .ForMember(x => x.Region, cfg => cfg.MapFrom(x => x.Region.Name))
                .ForMember(x => x.Level, cfg => cfg.MapFrom(x => x.Level.Name))
                .ForMember(x => x.GuideName, cfg => cfg.MapFrom(x => x.Guide.Name))
                .ForMember(x => x.GuidePhoneNumber, cfg => cfg.MapFrom(x => x.Guide.PhoneNumber))
                .ForMember(x => x.Participants, cfg => cfg.MapFrom(x => x.Users.Count));
               
            this.CreateMap<Event, EventServiceModel>()
                .ForMember(x => x.Region, cfg => cfg.MapFrom(x => x.Region.Name))
                .ForMember(x => x.Level, cfg => cfg.MapFrom(x => x.Level.Name))
                .ForMember(x => x.GuideName, cfg => cfg.MapFrom(x => x.Guide.Name))
                .ForMember(x => x.Participants, cfg => cfg.MapFrom(x => x.Users.Count));

        }
    }
}

