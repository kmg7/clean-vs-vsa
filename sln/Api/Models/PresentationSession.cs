namespace API.Models;

public class PresentationSession
{
    public Guid Id { get; set; }
    public Guid PresentationId { get; set; }
    public string Title { get; set; }
    public string RoomCode { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}

