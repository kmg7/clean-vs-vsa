using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Common.Exceptions;
using DPoll.Application.Features.Presentations;
using DPoll.Application.Features.Users;
using MediatR;

namespace DPoll.Application.Features.Presentations.Commands.UpdatePresentation;
public class UpdatePresentationHandler(
    IUsersRepository UsersRepository,
    IPresentationsRepository PresentationsRepository) : IRequestHandler<UpdatePresentationCommand, bool>
{
    public async Task<bool> Handle(UpdatePresentationCommand request, CancellationToken cancellationToken)
    {
        if (!await PresentationsRepository.PresentationExists(request.Id, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Presentation);
        }

        if (!await UsersRepository.UserExists(request.UserId, cancellationToken))
        {
            NotFoundException.Throw(EntityType.User);
        }

        return await PresentationsRepository
            .UpdatePresentation(request.Id, request.UserId, request.Title, cancellationToken);
    }
}
