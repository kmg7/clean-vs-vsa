namespace Infrastructure.Databases.UserPresentations.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

internal class PresentationConfiguration : EntityConfiguration<Presentation>
{
    public override void Configure(EntityTypeBuilder<Presentation> builder)
    {
        base.Configure(builder);
        builder.Property(p => p.Slides).HasColumnType("jsonb[]").IsRequired();
    }
}
