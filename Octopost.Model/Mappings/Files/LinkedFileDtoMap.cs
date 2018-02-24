namespace Octopost.Model.Mappings.Files
{
    using AutoMapper;
    using Octopost.Model.Data;
    using Octopost.Model.Dto;

    public class LinkedFileDtoMap : Profile
    {
        public LinkedFileDtoMap()
        {
            this.CreateMap<CreateLinkedFileDto, File>()
                .ForMember(x => x.Link, x => x.MapFrom(p => p.Link))
                .ForMember(x => x.FileName, x => x.Ignore())
                .ForMember(x => x.ContentType, x => x.Ignore())
                .ForMember(x => x.Created, x => x.Ignore())
                .ForMember(x => x.Data, x => x.Ignore())
                .ForMember(x => x.Id, x => x.Ignore());
        }
    }
}
