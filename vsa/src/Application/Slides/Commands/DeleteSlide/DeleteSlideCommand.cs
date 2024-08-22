namespace Application.Slides.Commands.DeleteSlide;

using MediatR;

public class DeleteSlideCommand : IRequest<bool>
{
    public Guid Id { get; init; }
}
