namespace Application.Presentations.Commands.CreatePresentation;

using Entities;
using MediatR;

public class CreatePresentationCommand : IRequest<Presentation>
{
    public Guid UserId { get; init; }
    public string Title { get; init; }
}
