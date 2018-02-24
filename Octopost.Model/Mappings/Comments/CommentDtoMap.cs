namespace Octopost.Model.Mappings.Comments
{
    using AutoMapper;
    using Octopost.Model.Data;
    using Octopost.Model.Dto;

    public class CommentDtoMap : Profile
    {
        public CommentDtoMap()
        {
            this.CreateMap<Comment, CommentDto>()
                .ForMember(x => x.Created, x => x.MapFrom(p => p.Created))
                .ForMember(x => x.Id, x => x.MapFrom(p => p.Id))
                .ForMember(x => x.Latitude, x => x.MapFrom(p => p.Latitude))
                .ForMember(x => x.LocationName, x => x.Ignore())
                .ForMember(x => x.Longitude, x => x.MapFrom(p => p.Longitude))
                .ForMember(x => x.Text, x => x.MapFrom(p => p.Text))
                .ForMember(x => x.PostId, x => x.MapFrom(p => p.PostId))
                .ReverseMap();
        }
    }
}
