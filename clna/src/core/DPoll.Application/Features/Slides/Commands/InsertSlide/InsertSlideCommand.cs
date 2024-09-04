using Dpoll.Domain.Entities;
using MediatR;
using System.Text.Json;

namespace DPoll.Application.Features.Slides.Commands.InsertSlide;
public class InsertSlideCommand : IRequest<Slide>
{
    public Guid PresentationId { get; init; }
    public int Index { get; set; }
    public string Type { get; set; }
    public JsonDocument Content { get; init; }
}
