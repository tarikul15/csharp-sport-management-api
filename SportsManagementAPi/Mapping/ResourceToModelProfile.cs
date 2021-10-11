using System;
using AutoMapper;
using SportsManagementAPi.Domain.Models;


namespace SportsManagementAPi.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateManagerRequest, Manager>()
                .ForMember(m => m.Id, opt => opt.MapFrom(c => Guid.NewGuid()));

            CreateMap<CreateTeamRequest, Team>()
                .ForMember(t => t.Id, opt => opt.MapFrom(c => Guid.NewGuid()));

            CreateMap<CreatePlayerRequest, Player>()
                .ForMember(p => p.Id, opt => opt.MapFrom(c => Guid.NewGuid()));

            CreateMap<CreateScheduleRequest, Schedule>()
                .ForMember(s => s.GameId, opt => opt.MapFrom(c => Guid.NewGuid()));

            CreateMap<CreateResultRequest, Result>();
        }
    }
}