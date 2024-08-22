namespace Application.Slides.Queries.GetSlides;

using Common.Enums;
using Common.Exceptions;
using Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetSlidesHandler(ISlidesRepository repository) : IRequestHandler<GetSlidesQuery, List<Slide>>
{
    public async Task<List<Slide>> Handle(GetSlidesQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetSlides(request.PresentationId, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Slide);

        return result;
    }
}
