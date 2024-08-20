using System.Text.Json;

namespace Infrastructure.Databases.UserPresentations.Models;

internal record Presentation : Entity
{
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public List<JsonDocument> Slides { get; set; }
}

