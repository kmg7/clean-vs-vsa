using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Common.Exceptions;
using MediatR;

namespace DPoll.Application.Features.Presentations.Commands.DeletePresentation;
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
