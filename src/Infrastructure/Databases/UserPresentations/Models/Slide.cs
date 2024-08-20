using System.Text.Json;

namespace Infrastructure.Databases.UserPresentations.Models;

internal class Slide
{
    public string Type { get; set; }
    public JsonDocument Content { get; set; }
}
