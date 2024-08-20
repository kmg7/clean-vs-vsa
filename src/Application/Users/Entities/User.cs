namespace Application.Users.Entities;
public record User(Guid Id, string Username, string Email, string ClerkId, bool IsActive)
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
