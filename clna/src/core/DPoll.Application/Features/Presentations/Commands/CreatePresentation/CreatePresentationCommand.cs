using Dpoll.Domain.Entities;
using MediatR;

namespace DPoll.Application.Features.Presentations.Commands.CreatePresentation;
public class CreatePresentationCommand : IRequest<Presentation>
{
    public Guid UserId { get; init; }
    public string Title { get; init; }
}
