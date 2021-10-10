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

            CreateMap<AccessToken, AccessTokenResource>()
                .ForMember(a => a.AccessToken, opt => opt.MapFrom(a => a.Token))
                .ForMember(a => a.RefreshToken, opt => opt.MapFrom(a => a.RefreshToken.Token))
                .ForMember(a => a.Expiration, opt => opt.MapFrom(a => a.Expiration));
        }
    }
}