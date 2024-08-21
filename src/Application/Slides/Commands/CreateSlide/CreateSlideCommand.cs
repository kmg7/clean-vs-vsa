namespace Application.Slides.Commands.CreateSlide;

using Entities;
using MediatR;
using System.Text.Json;

public class CreateSlideCommand : IRequest<Slide>
{
    public Guid PresentationId { get; init; }
    public string Type { get; set; }
    public JsonDocument Content { get; init; }
}
