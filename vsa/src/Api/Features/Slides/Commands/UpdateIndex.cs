namespace Api.Features.Slides.Commands;

using Api.Common;
using Api.Common.Exceptions;
using Api.Features.Slides.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

public partial class SlidesController : ApiControllerBase
{
    [HttpPatch("/api/presentation/{presentationId:guid}/slides/index")]
    public async Task<IActionResult> UpdateIndex([FromRoute] Guid presentationId, [FromRoute] Guid id, [FromBody] UpdateSlideIndexRequest request)
    {
        try
        {
            _ = await Mediator.Send(new UpdateIndexCommand
            {
                Id = id,
                Index = request.Index
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

public class UpdateIndexCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public int Index { get; set; }
}

public class UpdateSlideIndexRequest
{
    public int Index { get; set; }

}

public class UpdateSlideIndexRequestValidator : AbstractValidator<UpdateSlideIndexRequest>
{
    public UpdateSlideIndexRequestValidator()
    {
        _ = RuleFor(r => r.Index)
            .GreaterThanOrEqualTo(0)
            .WithMessage("A Slide index cannot be negative.");
    }
}


public class UpdateIndexCommandHandler(
    ISlidesRepository SlidesRepository) : IRequestHandler<UpdateIndexCommand, bool>
{
    public async Task<bool> Handle(UpdateIndexCommand request, CancellationToken cancellationToken)
    {
        return await SlidesRepository
            .UpdateSlideIndex(request.Id, request.Index, cancellationToken);
    }
}
