namespace Api.Features.Slides.Commands;

using Api.Common;
using Api.Common.Exceptions;
using Api.Features.Slides.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

public partial class SlidesController : ApiControllerBase
{
    [HttpDelete("/api/presentation/{presentationId:guid}/slides/{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        try
        {
            _ = await Mediator.Send(new DeleteCommand
            {
                Id = id,
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

public class DeleteCommand : IRequest<bool>
{
    public Guid Id { get; init; }
}

public class DeleteCommandHandler(ISlidesRepository slidesRepository)
    : IRequestHandler<DeleteCommand, bool>
{
    public async Task<bool> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        return await slidesRepository
            .DeleteSlide(request.Id, cancellationToken);
    }
}
