namespace Application.Presentations.Commands.DeletePresentation;

using System.Threading;
using System.Threading.Tasks;
using Application.Presentations;
using Common.Enums;
using Common.Exceptions;
using MediatR;

public class DeletePresentationHandler(IPresentationsRepository repository) : IRequestHandler<DeletePresentationCommand, bool>
{
    public async Task<bool> Handle(DeletePresentationCommand request, CancellationToken cancellationToken)
    {
        if (!await repository.PresentationExists(request.Id, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Presentation);
        }

        return await repository.DeletePresentation(request.Id, cancellationToken);
    }
}
