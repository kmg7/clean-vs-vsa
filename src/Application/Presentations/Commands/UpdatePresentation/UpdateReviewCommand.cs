namespace Application.Presentations.Commands.UpdatePresentation;

using MediatR;
using System.Text.Json;

public class UpdatePresentationCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; init; }
}
