namespace Application.Presentations.Commands.UpdatePresentation;

using System.Threading;
using System.Threading.Tasks;
using Application.Presentations;
using Application.Users;
using Common.Enums;
using Common.Exceptions;
using MediatR;

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
