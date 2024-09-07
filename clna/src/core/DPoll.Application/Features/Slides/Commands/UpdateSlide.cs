using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Entities;
using DPoll.Application.Shared;
using DPoll.Domain.Common.Errors;
using MediatR;
using System.Text.Json;

namespace DPoll.Application.Features.Slides.Commands;

public class UpdateSlideCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
    public string? Type { get; set; }
    public bool? IsVisible { get; set; }
    public JsonDocument? Content { get; init; }
}

public class UpdateSlideHandler(
    ISlidesRepository SlidesRepository) : IRequestHandler<UpdateSlideCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateSlideCommand request, CancellationToken token)
    {
        var slide = await SlidesRepository.GetSlideById(request.Id, token);
        if (slide is null)
            return Result<bool>.Failure(Error.NotFound(EntityType.Slide));

        var notChanged = !UpdateFields(request, slide);
        if (notChanged)
            return Result<bool>.Success(true);

        var response = await SlidesRepository.UpdateSlide(slide, token);

        return Result<bool>.Success(response);
    }
    private static bool UpdateFields(UpdateSlideCommand command, Slide slide)
    {
        var changed = false;
        if (command.Type is not null && command.Type != slide.Type)
        {
            slide.Type = command.Type;
            changed = true;
        }
        if (command.IsVisible is not null && command.IsVisible != slide.IsVisible)
        {
            slide.IsVisible = command.IsVisible.Value;
            changed = true;
        }
        if (command.Content is not null && command.Content != slide.Content)
        {
            slide.Content = command.Content;
            changed = true;
        }
        return changed;
    }
}
