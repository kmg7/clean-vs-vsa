using Api.Common.Extensions;
using Api.Common.Persistence;
using System.Text.Json;

namespace Api.Features.Slides.Entities;

public class Slide : Entity
{
    public Guid PresentationId { get; set; } = Guid.Empty;
    public int Index { get; set; } = -1;
    public string Type { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;
    public JsonDocument Content { get; set; } = JsonDocumentExtensions.Empty;
}