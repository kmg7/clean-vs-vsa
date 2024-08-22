using System.Text.Json;

namespace Infrastructure.Databases.UserPresentations.Models;

internal record Presentation : EntityWithTimeStamp
{
    public Guid UserId { get; set; }
    public string Title { get; set; }
}

