namespace Infrastructure.Databases.UserPresentations.Models;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal abstract record Entity
{
    public Guid Id { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; set; }
}
