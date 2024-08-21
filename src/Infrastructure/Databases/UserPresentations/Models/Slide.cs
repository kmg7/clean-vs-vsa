using System.Text.Json;

namespace Infrastructure.Databases.UserPresentations.Models;

internal record Slide : Entity
{
    public Guid PresentationId { get; set; }
    public int Index { get; set; }
    public bool IsVisible { get; set; }
    public string Type { get; set; }
    public JsonDocument Content { get; set; }
}
