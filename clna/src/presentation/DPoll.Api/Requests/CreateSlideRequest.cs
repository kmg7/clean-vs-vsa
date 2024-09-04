using System.Text.Json;

namespace Dpoll.Api.Requests;
public class CreateSlideRequest
{
    public string Type { get; set; }
    public JsonDocument Content { get; set; }
}
