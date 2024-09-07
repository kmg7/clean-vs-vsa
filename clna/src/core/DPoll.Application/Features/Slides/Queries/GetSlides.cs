using Dpoll.Domain.Entities;
using DPoll.Application.Shared;
using MediatR;

namespace DPoll.Application.Features.Slides.Queries;

public class GetSlidesQuery : IRequest<Result<List<Slide>>>
{
    public Guid PresentationId { get; init; }
}
public class GetSlides(ISlidesRepository repository) : IRequestHandler<GetSlidesQuery, Result<List<Slide>>>
{
    public async Task<Result<List<Slide>>> Handle(GetSlidesQuery request, CancellationToken token)
    {
        var slides = await repository.GetSlides(s => s.PresentationId == request.PresentationId, token);

        return Result<List<Slide>>.Success(slides.ToList());
    }
}
