namespace Application.Presentations.Queries.GetPresentationById;

using Entities;
using MediatR;

public class GetPresentationByIdQuery : IRequest<Presentation>
{
    public Guid Id { get; init; }
}
