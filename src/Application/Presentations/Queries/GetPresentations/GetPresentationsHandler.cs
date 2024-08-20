namespace Application.Presentations.Queries.GetPresentations;

using System.Threading;
using System.Threading.Tasks;
using Application.Presentations;
using Entities;
using MediatR;

public class GetPresentationsHandler(IPresentationsRepository repository) : IRequestHandler<GetPresentationsQuery, List<Presentation>>
{
    public async Task<List<Presentation>> Handle(GetPresentationsQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetPresentations(cancellationToken);
    }
}
