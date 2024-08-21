using System.Text.Json;

namespace Application.Presentations.Entities;

public record Presentation(Guid Id, Guid UserId, string Title)
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
