using Dpoll.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DPoll.Persistence.Databases.UserPresentations.Configuration;
internal class SlideConfiguration : EntityConfiguration<Slide>
{
    public override void Configure(EntityTypeBuilder<Slide> builder)
    {
        base.Configure(builder);
        _ = builder.Property(p => p.Content).HasColumnType("jsonb").IsRequired();
    }
}
