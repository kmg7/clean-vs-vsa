using MediatR;

namespace DPoll.Application.Features.Slides.Commands.UpdateSlideIndex;
public class UpdateSlideIndexCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public int Index { get; set; }
}
