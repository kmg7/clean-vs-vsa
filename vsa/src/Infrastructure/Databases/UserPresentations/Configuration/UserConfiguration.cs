namespace Infrastructure.Databases.UserPresentations.Configuration;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

internal class UserConfiguration : EntityWithTimeStampConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
    }
}
