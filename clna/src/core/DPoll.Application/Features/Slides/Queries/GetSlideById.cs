using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Entities;
using DPoll.Application.Shared;
using DPoll.Domain.Common.Errors;
using MediatR;

namespace DPoll.Application.Features.Slides.Queries;
public class GetSlideByIdQuery : IRequest<Result<Slide>>
{
    public Guid Id { get; init; }
}

public class GetSlideById(ISlidesRepository repository) : IRequestHandler<GetSlideByIdQuery, Result<Slide>>
{
    public async Task<Result<Slide>> Handle(GetSlideByIdQuery request, CancellationToken cancellationToken)
    {
        var slide = await repository.GetSlideById(request.Id, cancellationToken);
        if (slide is null)
            return Result<Slide>.Failure(Error.NotFound(EntityType.Slide));

        return Result<Slide>.Success(slide);
    }
}
