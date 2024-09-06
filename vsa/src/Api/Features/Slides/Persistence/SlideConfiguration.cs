namespace Api.Features.Slides.Persistence;

using Api.Common.Persistence;
using Api.Features.Slides.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class SlideConfiguration : EntityConfiguration<Slide>
{
    public override void Configure(EntityTypeBuilder<Slide> builder)
    {
        base.Configure(builder);
        builder.Property(p => p.Content).HasColumnType("jsonb").IsRequired();
    }
}
