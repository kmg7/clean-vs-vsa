﻿using System.Diagnostics.CodeAnalysis;

namespace Api.Common.Persistence;

[ExcludeFromCodeCoverage]
public abstract class Entity
{
    public Guid Id { get; set; } = Guid.Empty;
}

[ExcludeFromCodeCoverage]
public abstract class EntityWithTimeStamp : Entity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}