using System.Linq;
using AutoMapper;
using SportsManagementAPi.Domain.Models;
using SportsManagementAPi.Domain.Security;


namespace SportsManagementAPi.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Manager, ManagerResource>();

            CreateMap<Team, TeamResource>();

            CreateMap<Player, PlayerResource>();

            CreateMap<PlayerResource, Player>()
                .ForMember(p=>p.Id, opt=>opt.MapFrom(pr=>pr.Id))
                .ForMember(p => p.ManagerId, opt => opt.MapFrom(pr => pr.ManagerId))
                .ForMember(p => p.Name, opt => opt.MapFrom(pr => pr.Name))
                .ForMember(p => p.Details, opt => opt.MapFrom(pr => pr.Details))
                .ForMember(p => p.TeamId, opt => opt.MapFrom(pr => pr.TeamId));

            CreateMap<Schedule, ScheduleResource>();
            CreateMap<ScheduleResource, Schedule>();
                

            CreateMap<Schedule, ScheduleAndResultResources>();

            CreateMap<Result, ResultResource>();
            CreateMap<ResultResource, Result>();

            CreateMap<AccessToken, AccessTokenResource>()
                .ForMember(a => a.AccessToken, opt => opt.MapFrom(a => a.Token))
                .ForMember(a => a.RefreshToken, opt => opt.MapFrom(a => a.RefreshToken.Token))
                .ForMember(a => a.Expiration, opt => opt.MapFrom(a => a.Expiration));
        }
    }
}