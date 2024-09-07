using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Entities;
using DPoll.Application.Shared;
using DPoll.Domain.Common.Errors;
using MediatR;

namespace DPoll.Application.Features.Slides.Queries;
public class GetSlideByIndexQuery : IRequest<Result<Slide>>
{
    public Guid PresentationId { get; init; }
    public int Index { get; set; }
}

public class GetSlideByIndexHandler(ISlidesRepository repository) : IRequestHandler<GetSlideByIndexQuery, Result<Slide>>
{
    public async Task<Result<Slide>> Handle(GetSlideByIndexQuery request, CancellationToken cancellationToken)
    {
        var slide = await repository.GetSlideByIndex(request.PresentationId, request.Index, cancellationToken);
        if (slide is null)
            return Result<Slide>.Failure(Error.NotFound(EntityType.Slide));

        return Result<Slide>.Success(slide);
    }
}
