using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Entities;
using DPoll.Application.Features.Users;
using DPoll.Application.Shared;
using DPoll.Domain.Common.Errors;
using MediatR;

namespace DPoll.Application.Features.Presentations.Commands;

public class UpdatePresentationCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? Title { get; init; }
}

public class UpdatePresentationHandler(
    IUsersRepository UsersRepository,
    IPresentationsRepository PresentationsRepository) : IRequestHandler<UpdatePresentationCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdatePresentationCommand request, CancellationToken cancellationToken)
    {
        var presentation = await PresentationsRepository.GetPresentationById(request.Id, cancellationToken);

        if (presentation is null)
            return Result<bool>.Failure(Error.NotFound(EntityType.Presentation));

        var userNotExists = !await UsersRepository.UserExists(request.UserId, cancellationToken);
        if (userNotExists)
            return Result<bool>.Failure(Error.NotFound(EntityType.User));

        var notChanged = !UpdateFields(request, presentation);
        if (notChanged)
            return Result<bool>.Success(true);

        var result = await PresentationsRepository
            .UpdatePresentation(presentation, cancellationToken);

        return Result<bool>.Success(result);
    }

    private static bool UpdateFields(UpdatePresentationCommand command, Presentation presentation)
    {
        var changed = false;
        if (command.Title is not null && command.Title != presentation.Title)
        {
            presentation.Title = command.Title;
            changed = true;
        }

        return changed;
    }
}


