using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Entities;
using DPoll.Application.Features.Users;
using DPoll.Application.Shared;
using DPoll.Domain.Common.Errors;
using MediatR;

namespace DPoll.Application.Features.Presentations.Commands;
public class CreatePresentationCommand : IRequest<Result<Presentation>>
{
    public Guid UserId { get; init; }
    public string Title { get; init; }
}

public class CreatePresentationHandler(
    IUsersRepository UsersRepository,
    IPresentationsRepository PresentationsRepository) : IRequestHandler<CreatePresentationCommand, Result<Presentation>>
{
    public async Task<Result<Presentation>> Handle(CreatePresentationCommand request, CancellationToken cancellationToken)
    {
        var userNotExists = !await UsersRepository.UserExists(request.UserId, cancellationToken);
        if (userNotExists)
            return Result<Presentation>.Failure(Error.NotFound(EntityType.User));

        var presentation = new Presentation
        {
            UserId = request.UserId,
            Title = request.Title
        };
        presentation = await PresentationsRepository
            .CreatePresentation(presentation, cancellationToken);

        return Result<Presentation>.Success(presentation);
    }
}
