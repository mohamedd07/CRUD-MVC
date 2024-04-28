using AutoMapper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Demo.PL.Helpers.Profiles
{
    public class RoleProfile:Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleViewModeL,IdentityRole>()
                .ForMember(d => d.Name , O => O.MapFrom(S => S.RoleName)).ReverseMap();
            
        }
    }
}
