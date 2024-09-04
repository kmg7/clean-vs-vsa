using MediatR;
using System.Text.Json;

namespace DPoll.Application.Features.Slides.Commands.UpdateSlide;
public class UpdateSlideCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public bool IsVisible { get; set; }
    public JsonDocument Content { get; init; }
}
