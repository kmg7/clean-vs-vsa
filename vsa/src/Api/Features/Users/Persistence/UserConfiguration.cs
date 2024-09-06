using Api.Common.Persistence;
using Api.Features.Users.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Features.Users.Persistence;

internal class UserConfiguration : EntityWithTimeStampConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
    }
}