using MediatR;

namespace DPoll.Application.Features.Presentations.Commands.UpdatePresentation;
public class UpdatePresentationCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; init; }
}
