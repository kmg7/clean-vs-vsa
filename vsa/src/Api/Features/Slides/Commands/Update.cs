namespace Api.Features.Slides.Commands;

using Api.Common;
using Api.Common.Exceptions;
using Api.Features.Slides.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public partial class SlidesController : ApiControllerBase
{
    [HttpPut("/api/presentation/{presentationId:guid}/slides/{id:int}")]
    public async Task<IActionResult> Update([FromRoute] Guid presentationId, [FromRoute] Guid id, [FromBody] UpdateSlideRequest request)
    {
        try
        {
            _ = await Mediator.Send(new UpdateCommand
            {
                Id = id,
                Type = request.Type,
                Content = request.Content,
                IsVisible = request.IsVisible
            });

            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}

public class UpdateCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public bool IsVisible { get; set; }
    public JsonDocument Content { get; init; }
}

public class UpdateSlideRequest
{
    public int Index { get; set; }
    public string Type { get; set; }
    public bool IsVisible { get; set; }
    public JsonDocument Content { get; set; }

}

public class UpdateSlideValidator : AbstractValidator<UpdateSlideIndexRequest>
{
    public UpdateSlideValidator()
    {
        _ = RuleFor(r => r.Index)
            .GreaterThanOrEqualTo(0)
            .WithMessage("A Slide index cannot be negative.");
    }
}

public class UpdateCommandHandler(
    ISlidesRepository SlidesRepository) : IRequestHandler<UpdateCommand, bool>
{
    public async Task<bool> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        return await SlidesRepository
            .UpdateSlide(request.Id, request.Type, request.IsVisible, request.Content, cancellationToken);
    }
}
