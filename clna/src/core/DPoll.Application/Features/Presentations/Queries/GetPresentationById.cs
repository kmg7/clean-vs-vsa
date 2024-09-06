using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Entities;
using DPoll.Application.Shared;
using DPoll.Domain.Common.Errors;
using MediatR;

namespace DPoll.Application.Features.Presentations.Queries;

public class GetPresentationByIdQuery : IRequest<Result<Presentation>>
{
    public Guid Id { get; init; }
}

public class GetPresentationByIdHandler(IPresentationsRepository repository) : IRequestHandler<GetPresentationByIdQuery, Result<Presentation>>
{
    public async Task<Result<Presentation>> Handle(GetPresentationByIdQuery request, CancellationToken cancellationToken)
    {
        var presentation = await repository.GetPresentationById(request.Id, cancellationToken);
        if (presentation is null)
            return Result<Presentation>.Failure(Error.NotFound(EntityType.Presentation));

        return Result<Presentation>.Success(presentation);
    }
}
