using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Common.Exceptions;
using Dpoll.Domain.Entities;
using DPoll.Application.Features.Presentations;
using DPoll.Application.Features.Users;
using MediatR;

namespace DPoll.Application.Features.Presentations.Commands.CreatePresentation;
public class CreatePresentationHandler(
    IUsersRepository UsersRepository,
    IPresentationsRepository PresentationsRepository) : IRequestHandler<CreatePresentationCommand, Presentation>
{
    public async Task<Presentation> Handle(CreatePresentationCommand request, CancellationToken cancellationToken)
    {
        if (!await UsersRepository.UserExists(request.UserId, cancellationToken))
        {
            NotFoundException.Throw(EntityType.User);
        }

        return await PresentationsRepository
            .CreatePresentation(request.UserId, request.Title, cancellationToken);
    }
}
