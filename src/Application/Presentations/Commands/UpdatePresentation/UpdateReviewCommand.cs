namespace Application.Presentations.Commands.UpdatePresentation;

using MediatR;

public class UpdatePresentationCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; init; }
}
