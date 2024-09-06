namespace Api.Features.Presentations.Queries;

using Api.Common;
using Api.Common.Enums;
using Api.Common.Exceptions;
using Api.Features.Presentations.Persistence;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

public partial class PresentationsController : ApiControllerBase
{
    [HttpGet("/api/presentations/{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        try
        {
            return Ok(await Mediator.Send(new GetByIdQuery
            {
                Id = id
            }));
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

public class GetByIdQuery : IRequest<Presentation>
{
    public Guid Id { get; init; }
}

public class GetByIdQueryHandler(IPresentationsRepository repository) : IRequestHandler<GetByIdQuery, Presentation>
{
    public async Task<Presentation> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetPresentationById(request.Id, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Presentation);

        return result;
    }
}
