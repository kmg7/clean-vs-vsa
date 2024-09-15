using System.Text.Json;

namespace Dpoll.Domain.Extensions;

public static class JsonDocumentExtensions
{
    public static readonly JsonDocument Empty = JsonDocument.Parse("{}");
}
