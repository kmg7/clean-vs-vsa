namespace Application.Slides.Queries.GetSlides;

using Entities;
using MediatR;

public class GetSlidesQuery : IRequest<List<Slide>>
{
    public Guid PresentationId { get; init; }
}
