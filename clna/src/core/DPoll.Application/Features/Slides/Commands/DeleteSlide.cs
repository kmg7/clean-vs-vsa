using Dpoll.Domain.Common.Enums;
using DPoll.Application.Shared;
using DPoll.Domain.Common.Errors;
using MediatR;

namespace DPoll.Application.Features.Slides.Commands;

public class DeleteSlideCommand : IRequest<Result<bool>>
{
    public Guid Id { get; init; }
}

public class DeleteSlideHandler(ISlidesRepository SlidesRepository)
    : IRequestHandler<DeleteSlideCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteSlideCommand request, CancellationToken token)
    {
        var slide = await SlidesRepository.GetSlideById(request.Id, token);
        if (slide == null)
            return Result<bool>.Failure(Error.NotFound(EntityType.Slide));

        await SlidesRepository
           .DeleteSlide(request.Id, token);

        var slidesAfter = await SlidesRepository
            .GetSlides(s => s.PresentationId == slide.PresentationId, token);

        foreach (var slideAfter in slidesAfter) slideAfter.Index--;

        _ = await SlidesRepository.UpdateSlideRange(slidesAfter, token);

        return Result<bool>.Success(true);
    }
}
