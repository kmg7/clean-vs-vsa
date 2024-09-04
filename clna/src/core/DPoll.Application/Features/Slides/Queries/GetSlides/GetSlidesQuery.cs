using Dpoll.Domain.Entities;
using MediatR;

namespace DPoll.Application.Features.Slides.Queries.GetSlides;
public class GetSlidesQuery : IRequest<List<Slide>>
{
    public Guid PresentationId { get; init; }
}
