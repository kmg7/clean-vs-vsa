namespace Infrastructure.Databases.UserPresentations.Models;

internal record User : EntityWithTimeStamp
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string ClerkId { get; set; }
    public bool IsActive { get; set; }
}

