namespace Application.Slides.Commands.UpdateSlideIndex;

using MediatR;

public class UpdateSlideIndexCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public int Index { get; set; }
}
