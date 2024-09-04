using Dpoll.Domain.Entities;
using MediatR;

namespace DPoll.Application.Features.Presentations.Queries.GetPresentationById;
public class GetPresentationByIdQuery : IRequest<Presentation>
{
    public Guid Id { get; init; }
}
