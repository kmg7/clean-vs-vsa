using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Common.Exceptions;
using Dpoll.Domain.Entities;
using DPoll.Application.Features.Presentations;
using DPoll.Application.Features.Slides;
using MediatR;

namespace DPoll.Application.Features.Slides.Commands.CreateSlide;
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
