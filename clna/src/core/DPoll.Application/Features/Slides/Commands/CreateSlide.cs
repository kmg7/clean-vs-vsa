using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Entities;
using DPoll.Application.Features.Presentations;
using DPoll.Application.Shared;
using DPoll.Domain.Common.Errors;
using MediatR;
using System.Text.Json;

namespace DPoll.Application.Features.Slides.Commands;

public class CreateSlideCommand : IRequest<Result<Slide>>
{
    public Guid PresentationId { get; init; }
    public required string Type { get; set; }
    public required JsonDocument Content { get; init; }
}

public class CreateSlideHandler(IPresentationsRepository PresentationsRepository,
    ISlidesRepository SlidesRepository) : IRequestHandler<CreateSlideCommand, Result<Slide>>
{
    public async Task<Result<Slide>> Handle(CreateSlideCommand request, CancellationToken cancellationToken)
    {
        var presentationNotExists = 
            !await PresentationsRepository.PresentationExists(request.PresentationId, cancellationToken);

        if (presentationNotExists)
            return Result<Slide>.Failure(Error.NotFound(EntityType.Presentation));
        var slide = new Slide
        {
            PresentationId = request.PresentationId,
            Type = request.Type,
            Content = request.Content
        };

        var lastIndex = await SlidesRepository.GetLastSlideIndex(
            request.PresentationId, cancellationToken);
        slide.Index = lastIndex + 1;

        slide = await SlidesRepository.CreateSlide(slide, cancellationToken);
        return Result<Slide>.Success(slide);
    }
}
