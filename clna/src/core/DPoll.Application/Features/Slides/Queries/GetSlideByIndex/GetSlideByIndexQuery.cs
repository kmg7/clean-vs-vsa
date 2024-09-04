using Dpoll.Domain.Entities;
using MediatR;

namespace DPoll.Application.Features.Slides.Queries.GetSlideByIndex;
public class GetSlideByIndexQuery : IRequest<Slide>
{
    public Guid PresentationId { get; init; }
    public int Index { get; set; }
}
