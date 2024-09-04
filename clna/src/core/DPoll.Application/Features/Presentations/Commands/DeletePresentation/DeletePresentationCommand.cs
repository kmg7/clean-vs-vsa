using MediatR;

namespace DPoll.Application.Features.Presentations.Commands.DeletePresentation;
public class DeletePresentationCommand : IRequest<bool>
{
    public Guid Id { get; init; }
}
