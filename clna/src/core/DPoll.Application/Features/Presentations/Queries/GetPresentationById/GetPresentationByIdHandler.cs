using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Common.Exceptions;
using Dpoll.Domain.Entities;
using DPoll.Application.Features.Presentations;
using MediatR;

namespace DPoll.Application.Features.Presentations.Queries.GetPresentationById;
public class GetPresentationByIdHandler(IPresentationsRepository repository) : IRequestHandler<GetPresentationByIdQuery, Presentation>
{
    public async Task<Presentation> Handle(GetPresentationByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetPresentationById(request.Id, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Presentation);

        return result;
    }
}
