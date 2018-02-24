namespace Octopost.Model.Mappings.Files
{
    using AutoMapper;
    using Octopost.Model.Data;
    using Octopost.Model.Dto;

    public class FileDtoMap : Profile
    {
        public FileDtoMap()
        {
            this.CreateMap<File, FileDto>()
                .ForMember(x => x.ContentType, x => x.MapFrom(p => p.ContentType))
                .ForMember(x => x.Created, x => x.MapFrom(p => p.Created))
                .ForMember(x => x.Data, x => x.MapFrom(p => p.Data))
                .ForMember(x => x.Link, x => x.MapFrom(p => p.Link))
                .ForMember(x => x.FileName, x => x.MapFrom(p => p.FileName))
                .ForMember(x => x.Id, x => x.MapFrom(p => p.Id));
        }
    }
}
