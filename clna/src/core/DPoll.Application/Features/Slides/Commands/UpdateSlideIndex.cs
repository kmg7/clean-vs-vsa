using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Entities;
using DPoll.Application.Shared;
using DPoll.Domain.Common.Errors;
using MediatR;

namespace DPoll.Application.Features.Slides.Commands;
public class UpdateSlideIndexCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
    public int Index { get; set; }
}

public class UpdateSlideIndexHandler(
    ISlidesRepository SlidesRepository) : IRequestHandler<UpdateSlideIndexCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateSlideIndexCommand request, CancellationToken token)
    {
        var slide = await SlidesRepository.GetSlideById(request.Id, token);
        if (slide is null)
            return Result<bool>.Failure(Error.NotFound(EntityType.Slide));

        int index = request.Index;
        var notChanged = slide.Index == index;
        if (notChanged)
            return Result<bool>.Success(true);

        IQueryable<Slide> affectedSlides;
        int currentIndex = slide.Index;
        bool isDirectionDownwards = currentIndex < index;
        int indexModify = isDirectionDownwards ? -1 : 1;

        if (isDirectionDownwards)
            affectedSlides = await SlidesRepository.
                GetSlides(s => s.Index > currentIndex && s.Index <= index, token);
        else
            affectedSlides = await SlidesRepository.
                GetSlides(s => s.Index < currentIndex && s.Index >= index, token);

        foreach (var affectedSlide in affectedSlides) affectedSlide.Index += indexModify;
        slide.Index = index;
        affectedSlides = affectedSlides.Append(slide);

        _ = await SlidesRepository.UpdateSlideRange(affectedSlides, token);
        return Result<bool>.Success(true);
    }
}
