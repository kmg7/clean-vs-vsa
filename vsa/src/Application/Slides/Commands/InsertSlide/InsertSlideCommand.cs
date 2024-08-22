namespace Application.Slides.Commands.InsertSlide;

using Entities;
using MediatR;
using System.Text.Json;

public class InsertSlideCommand : IRequest<Slide>
{
    public Guid PresentationId { get; init; }
    public int Index { get; set; }
    public string Type { get; set; }
    public JsonDocument Content { get; init; }
}
