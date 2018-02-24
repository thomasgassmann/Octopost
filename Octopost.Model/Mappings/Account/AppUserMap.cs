namespace Octopost.Model.Mappings.Account
{
    using AutoMapper;
    using Octopost.Model.Data;
    using Octopost.Model.Dto;

    public class AppUserMap : Profile
    {
        public AppUserMap()
        {
            this.CreateMap<RegisterDto, AppUser>()
                .ForMember(x => x.UserName, x => x.MapFrom(p => p.Email))
                .ForMember(x => x.FirstName, x => x.MapFrom(p => p.FirstName))
                .ForMember(x => x.LastName, x => x.MapFrom(p => p.LastName));
        }
    }
}
