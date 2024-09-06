namespace Api.Features.Presentations.Commands;

using Api.Common;
using Api.Common.Enums;
using Api.Common.Exceptions;
using Api.Features.Presentations.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

public partial class PresentationsController : ApiControllerBase
{
    [HttpDelete("/api/presentations/{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        try
        {
            var response = await Mediator.Send(new DeleteCommand
            {
                Id = id
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

public class DeleteCommandHandler(IPresentationsRepository repository) : IRequestHandler<DeleteCommand, bool>
{
    public async Task<bool> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        if (!await repository.PresentationExists(request.Id, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Presentation);
        }

        return await repository.DeletePresentation(request.Id, cancellationToken);
    }
}
