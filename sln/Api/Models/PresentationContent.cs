using System.Text.Json;

namespace API.Models;
public class PresentationContent
{
    public Guid Id { get; set; }
    public Guid PresentationId { get; set; }
    public string Title { get; set; }
    public JsonDocument Slides { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}

