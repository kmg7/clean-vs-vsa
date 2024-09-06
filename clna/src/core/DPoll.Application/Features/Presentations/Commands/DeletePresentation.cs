using Dpoll.Domain.Common.Enums;
using DPoll.Application.Shared;
using DPoll.Domain.Common.Errors;
using MediatR;

namespace DPoll.Application.Features.Presentations.Commands;

public class DeletePresentationCommand : IRequest<Result<bool>>
{
    public Guid Id { get; init; }
}

public class DeletePresentationHandler(IPresentationsRepository repository) : IRequestHandler<DeletePresentationCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeletePresentationCommand request, CancellationToken cancellationToken)
    {
        var presentationNotExists = !await repository.PresentationExists(request.Id, cancellationToken);
        if (presentationNotExists)
            return Result<bool>.Failure(Error.NotFound(EntityType.Presentation));

        var isDeleted = await repository.DeletePresentation(request.Id, cancellationToken);
        return Result<bool>.Success(isDeleted);
    }
}
