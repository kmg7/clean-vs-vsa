using Dpoll.Domain.Entities;
using DPoll.Application.Shared;
using MediatR;

namespace DPoll.Application.Features.Presentations.Queries;

public class GetPresentationsQuery : IRequest<Result<List<Presentation>>> { }

public class GetPresentationsHandler(IPresentationsRepository repository) : IRequestHandler<GetPresentationsQuery, Result<List<Presentation>>>
{
    public async Task<Result<List<Presentation>>> Handle(GetPresentationsQuery request, CancellationToken cancellationToken)
    {
        var presentations = await repository.GetPresentations(cancellationToken);

        return Result<List<Presentation>>.Success(presentations);
    }
}
