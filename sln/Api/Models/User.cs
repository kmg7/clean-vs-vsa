namespace API.Models;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string EmailAddress { get; set; }
    public string ClerkId { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public bool IsActive { get; set; }
}

