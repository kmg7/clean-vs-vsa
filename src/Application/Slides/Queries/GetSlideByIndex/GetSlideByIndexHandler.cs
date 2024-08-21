namespace Application.Slides.Queries.GetSlideByIndex;

using Application.Slides.Entities;
using Common.Enums;
using Common.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


public class GetSlideByIndexHandler(ISlidesRepository repository) : IRequestHandler<GetSlideByIndexQuery, Slide>
{
    public async Task<Slide> Handle(GetSlideByIndexQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetSlideByIndex(request.PresentationId, request.Index, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Slide);

        return result;
    }
}
