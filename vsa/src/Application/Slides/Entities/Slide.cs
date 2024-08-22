using System.Text.Json;

namespace Application.Slides.Entities;

public record Slide(
    Guid Id,
    Guid PresentationId,
    int Index,
    string Type,
    bool IsVisible,
    JsonDocument Content);

