using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Entities;
using DPoll.Application.Features.Presentations;
using DPoll.Application.Shared;
using DPoll.Domain.Common.Errors;
using MediatR;
using System.Text.Json;

namespace DPoll.Application.Features.Slides.Commands;

public class InsertSlideCommand : IRequest<Result<Slide>>
{
    public int IndexOfSlideBefore { get; set; }
    public Guid PresentationId { get; init; }
    public string Type { get; set; }
    public JsonDocument Content { get; init; }
}

public class InsertSlideHandler(IPresentationsRepository PresentationsRepository,
    ISlidesRepository SlidesRepository) : IRequestHandler<InsertSlideCommand, Result<Slide>>
{
    public async Task<Result<Slide>> Handle(InsertSlideCommand request, CancellationToken token)
    {
        var presentationNotExists =
              !await PresentationsRepository.PresentationExists(request.PresentationId, token);

        if (presentationNotExists)
            return Result<Slide>.Failure(Error.NotFound(EntityType.Presentation));

        var slides = await SlidesRepository.
            GetSlides(s => s.PresentationId == request.PresentationId, token);

        var slideBefore = slides.FirstOrDefault(s => s.Index == request.IndexOfSlideBefore);
        if (slideBefore == null)
            return Result<Slide>.Failure(Error.NotFound(EntityType.Slide)); // Todo application error here

        var slide = new Slide
        {
            PresentationId = request.PresentationId,
            Type = request.Type,
            Content = request.Content,
            Index = request.IndexOfSlideBefore + 1
        };
        
        var slidesAfter = slides.Where(s => s.Index > request.IndexOfSlideBefore);
        foreach (var slideAfter in slidesAfter) slideAfter.Index++;

        slide = await SlidesRepository.CreateSlide(slide, token);
        await SlidesRepository.UpdateSlideRange(slidesAfter, token);

        return Result<Slide>.Success(slide);
    }
}
