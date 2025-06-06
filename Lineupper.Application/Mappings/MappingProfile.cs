using AutoMapper;
using Lineupper.Application.Dto;
using Lineupper.Domain.Models;

namespace Lineupper.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User hierarchy
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Participant, ParticipantDto>()
                .IncludeBase<User, UserDto>()
                .ReverseMap();
            CreateMap<Organizer, OrganizerDto>()
                .IncludeBase<User, UserDto>()
                .ReverseMap();

            // Simple entities
            CreateMap<Festival, FestivalDto>().ReverseMap();
            CreateMap<Band, BandDto>().ReverseMap();
            CreateMap<Vote, VoteDto>().ReverseMap();
            CreateMap<ScheduleItem, ScheduleItemDto>().ReverseMap();
            CreateMap<User, RegisterUserDto>().ReverseMap();
        }
    }
}

