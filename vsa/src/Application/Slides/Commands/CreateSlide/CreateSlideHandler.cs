namespace Application.Slides.Commands.CreateSlide;

using Application.Presentations;
using Common.Enums;
using Common.Exceptions;
using Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class CreateSlideHandler(IPresentationsRepository PresentationsRepository,
    ISlidesRepository SlidesRepository) : IRequestHandler<CreateSlideCommand, Slide>
{
    public async Task<Slide> Handle(CreateSlideCommand request, CancellationToken cancellationToken)
    {
        if (!await PresentationsRepository.PresentationExists(request.PresentationId, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Presentation);
        }

        return await SlidesRepository
            .CreateSlide(request.PresentationId, request.Type, request.Content, cancellationToken);
    }
}
