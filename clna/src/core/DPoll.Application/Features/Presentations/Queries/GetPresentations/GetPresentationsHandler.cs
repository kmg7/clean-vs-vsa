using Dpoll.Domain.Entities;
using MediatR;

namespace DPoll.Application.Features.Presentations.Queries.GetPresentations;
public class GetPresentationsHandler(IPresentationsRepository repository) : IRequestHandler<GetPresentationsQuery, List<Presentation>>
{
    public async Task<List<Presentation>> Handle(GetPresentationsQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetPresentations(cancellationToken);
    }
}
