using System.Text.Json;

namespace Api.Requests;

public class CreatePresentationContentRequest
{
    public Guid PresentationId { get; set; }
    public string Title { get; set; }
    public JsonDocument Slides { get; set; }
}
