using System.Text.Json;

namespace Presentation.Requests;

public class UpdateSlideRequest
{
    public int Index { get; set; }
    public string Type { get; set; }
    public bool IsVisible { get; set; }
    public JsonDocument Content { get; set; }

}
