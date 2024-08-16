namespace Api.Requests;

public class CreatePresentationSessionRequest
{
    public Guid PresentationId { get; set; }
    public string Title { get; set; }
    public string RoomCode { get; set; }
}
