namespace Infrastructure.Databases.UserPresentations.Mapping;

using AutoMapper;
using Application = Application.Presentations.Entities;
using Infrastructure = Models;

internal class PresentationMappingProfile : Profile
{
    public PresentationMappingProfile()
    {
        _ = CreateMap<Application.Presentation, Infrastructure.Presentation>()
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ReverseMap();
    }
}
