namespace Application.Presentations.Commands.DeletePresentation;

using MediatR;

public class DeletePresentationCommand : IRequest<bool>
{
    public Guid Id { get; init; }
}
