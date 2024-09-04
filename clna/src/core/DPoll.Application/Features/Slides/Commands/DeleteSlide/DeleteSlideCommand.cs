using MediatR;

namespace DPoll.Application.Features.Slides.Commands.DeleteSlide;
public class DeleteSlideCommand : IRequest<bool>
{
    public Guid Id { get; init; }
}
