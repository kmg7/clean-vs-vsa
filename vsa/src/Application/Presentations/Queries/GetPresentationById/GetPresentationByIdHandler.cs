namespace Application.Presentations.Queries.GetPresentationById;

using System.Threading;
using System.Threading.Tasks;
using Application.Presentations;
using Common.Enums;
using Common.Exceptions;
using Entities;
using MediatR;

public class GetPresentationByIdHandler(IPresentationsRepository repository) : IRequestHandler<GetPresentationByIdQuery, Presentation>
{
    public async Task<Presentation> Handle(GetPresentationByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetPresentationById(request.Id, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Presentation);

        return result;
    }
}
