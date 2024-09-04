using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Common.Exceptions;
using Dpoll.Domain.Entities;
using MediatR;

namespace DPoll.Application.Features.Slides.Queries.GetSlideByIndex;
public class GetSlideByIndexHandler(ISlidesRepository repository) : IRequestHandler<GetSlideByIndexQuery, Slide>
{
    public async Task<Slide> Handle(GetSlideByIndexQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetSlideByIndex(request.PresentationId, request.Index, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Slide);

        return result;
    }
}
