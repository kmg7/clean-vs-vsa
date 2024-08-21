namespace Presentation.Requests;
using System.Text.Json;


public class CreateSlideRequest
{
    public string Type { get; set; }
    public JsonDocument Content { get; set; }
}
