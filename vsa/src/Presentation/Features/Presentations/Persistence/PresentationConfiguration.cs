using Api.Common.Persistence;
using Api.Features.Presentations.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Features.Presentations.Persistence;

internal class PresentationConfiguration : EntityWithTimeStampConfiguration<Presentation>
{
    public override void Configure(EntityTypeBuilder<Presentation> builder)
    {
        base.Configure(builder);
    }
}
