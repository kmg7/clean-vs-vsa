using Api.Common.Persistence;

namespace Api.Features.Presentations.Entities;

public class Presentation : EntityWithTimeStamp
{
    public Guid UserId { get; set; } = Guid.Empty;
    public string Title { get; set; } = string.Empty;
}
