﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Common.Persistence
{
    internal abstract class EntityConfiguration<T> : IEntityTypeConfiguration<T>
        where T : Entity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            _ = builder.HasKey(e => e.Id);
            _ = builder.Property(m => m.Id).ValueGeneratedOnAdd().IsRequired();
        }
    }
    internal abstract class EntityWithTimeStampConfiguration<T> : IEntityTypeConfiguration<T>
            where T : EntityWithTimeStamp
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            _ = builder.HasKey(e => e.Id);
            _ = builder.Property(m => m.Id).ValueGeneratedOnAdd().IsRequired();
            _ = builder.Property(m => m.CreatedAt).ValueGeneratedOnAdd().IsRequired();
            _ = builder.Property(m => m.UpdatedAt).ValueGeneratedOnAdd().IsRequired();
        }
    }
}
