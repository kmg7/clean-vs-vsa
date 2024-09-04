using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Common.Exceptions;
using Dpoll.Domain.Entities;
using DPoll.Application.Features.Presentations;
using DPoll.Application.Features.Slides;
using MediatR;

namespace DPoll.Application.Features.Slides.Commands.InsertSlide;
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
