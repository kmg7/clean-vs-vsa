namespace Infrastructure.Databases.UserPresentations.Mapping;

using AutoMapper;
using Application = Application.Users.Entities;
using Infrastructure = Models;

internal class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        _ = CreateMap<Application.User, Infrastructure.User>()
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ReverseMap();
    }
}
