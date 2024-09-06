using System.Text.Json;

namespace Api.Common.Extensions;

public static class JsonDocumentExtensions
{
    public static readonly JsonDocument Empty = JsonDocument.Parse("{}");
}
