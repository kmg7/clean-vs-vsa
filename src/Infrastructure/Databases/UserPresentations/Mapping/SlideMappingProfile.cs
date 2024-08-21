namespace Infrastructure.Databases.UserPresentations.Mapping;

using AutoMapper;
using Application = Application.Slides.Entities;
using Infrastructure = Models;

internal class SlideMappingProfile : Profile
{
    public SlideMappingProfile()
    {
        _ = CreateMap<Application.Slide, Infrastructure.Slide>()
            .ReverseMap();
    }
}
