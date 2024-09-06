using Dpoll.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DPoll.Persistence.Databases.UserPresentations.Configuration;
internal class PresentationConfiguration : EntityWithTimeStampConfiguration<Presentation>
{
    public override void Configure(EntityTypeBuilder<Presentation> builder)
    {
        base.Configure(builder);
    }
}
