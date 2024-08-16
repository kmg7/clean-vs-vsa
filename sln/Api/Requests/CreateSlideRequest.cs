using System.Text.Json.Nodes;

namespace Api.Requests;

public class CreateSlideRequest
{
    public string Index { get; set; }
    public JsonNode Slide { get; set; }
}
