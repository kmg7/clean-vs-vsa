namespace Application.Slides.Commands.InsertSlide;

using Application.Presentations;
using Common.Enums;
using Common.Exceptions;
using Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class InsertSlideHandler(IPresentationsRepository PresentationsRepository,
    ISlidesRepository SlidesRepository) : IRequestHandler<InsertSlideCommand, Slide>
{
    public async Task<Slide> Handle(InsertSlideCommand request, CancellationToken cancellationToken)
    {
        if (!await PresentationsRepository.PresentationExists(request.PresentationId, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Presentation);
        }

        return await SlidesRepository
            .InsertSlide(request.PresentationId, request.Index, request.Type, request.Content, cancellationToken);
    }
}
