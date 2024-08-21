namespace Application.Slides.Queries.GetSlideByIndex;

using Entities;
using MediatR;

public class GetSlideByIndexQuery : IRequest<Slide>
{
    public Guid PresentationId { get; init; }
    public int Index { get; set; }
}
