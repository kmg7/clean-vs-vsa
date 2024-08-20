namespace Infrastructure.Databases.UserPresentations.Mapping;

using AutoMapper;
using Application = Application.Presentations.Entities;
using Infrastructure = Models;

internal class PresentationMappingProfile : Profile
{
    public PresentationMappingProfile()
    {
        _ = CreateMap<Application.Presentation, Infrastructure.Presentation>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.Slides, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ReverseMap();
    }
}
