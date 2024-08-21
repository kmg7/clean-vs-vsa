namespace Infrastructure.Databases.UserPresentations.Configuration;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

internal class PresentationConfiguration : EntityWithTimeStampConfiguration<Presentation>
{
    public override void Configure(EntityTypeBuilder<Presentation> builder)
    {
        base.Configure(builder);
    }
}
