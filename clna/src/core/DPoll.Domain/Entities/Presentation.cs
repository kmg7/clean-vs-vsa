using DPoll.Domain.Entities;
using System;

namespace Dpoll.Domain.Entities;

public class Presentation : EntityWithTimeStamp
{
    public Guid UserId { get; set; } = Guid.Empty;
    public string Title { get; set; } = string.Empty;
}
