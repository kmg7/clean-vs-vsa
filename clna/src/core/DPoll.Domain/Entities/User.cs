using DPoll.Domain.Entities;

namespace Dpoll.Domain.Entities;

public class User : EntityWithTimeStamp
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ClerkId { get; set; } = string.Empty;
    public bool IsActive { get; set; } = false;
}
