using AutoMapper;
using WebApi.Dtos;
using Domain.Entities;

namespace WebApi.Profiles
{
    public class ProjectProfile : Profile

    {
        public ProjectProfile()
        {
            //get api/sources
            CreateMap<Tag, TagReadDto>();
            CreateMap<Source, SourceReadDto>();

            //get api/tags
            CreateMap<Tag, TagReadWithParentPropDto>();

            //post api/sources
            CreateMap<TagOnlyIdDto, Tag>();
            CreateMap<SourceCreateDto, Source>();

            //post api/tags
            CreateMap<TagCreateDto, Tag>();

            //patch api/tags
            CreateMap<TagUpdateDto, Tag>();
            CreateMap<Tag, TagUpdateDto>();

            CreateMap<Tag, TagOnlyIdDto>();

            //patch api/sources
            CreateMap<SourceUpdateDto, Source>();
            CreateMap<Source, SourceUpdateDto>();

        }
    }
}
