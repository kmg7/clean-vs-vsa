using Dpoll.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DPoll.Infrastructure.Databases.UserPresentations.Configuration;
internal class UserConfiguration : EntityWithTimeStampConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
    }
}
