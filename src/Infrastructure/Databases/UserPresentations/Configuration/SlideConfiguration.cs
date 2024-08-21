namespace Infrastructure.Databases.UserPresentations.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

internal class SlideConfiguration : EntityConfiguration<Slide>
{
    public override void Configure(EntityTypeBuilder<Slide> builder)
    {
        base.Configure(builder);
        builder.Property(p => p.Content).HasColumnType("jsonb").IsRequired();
    }
}
