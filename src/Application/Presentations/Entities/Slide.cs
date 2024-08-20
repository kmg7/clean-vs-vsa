using System.Text.Json;

namespace Application.Presentations.Entities;

public record Slide(string Type, JsonDocument Content);
