namespace Application.Presentations.Commands.CreatePresentation;

using System.Threading;
using System.Threading.Tasks;
using Users;
using Common.Enums;
using Common.Exceptions;
using Entities;
using MediatR;
using Application.Presentations;

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
