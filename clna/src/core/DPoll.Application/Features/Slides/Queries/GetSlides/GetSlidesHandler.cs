using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Common.Exceptions;
using Dpoll.Domain.Entities;
using DPoll.Application.Features.Slides;
using MediatR;

namespace DPoll.Application.Features.Slides.Queries.GetSlides;
public class GetSlidesHandler(ISlidesRepository repository) : IRequestHandler<GetSlidesQuery, List<Slide>>
{
    public async Task<List<Slide>> Handle(GetSlidesQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetSlides(request.PresentationId, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Slide);

        return result;
    }
}
