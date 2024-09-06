namespace Api.Features.Slides.Queries;

using Api.Common;
using Api.Features.Slides.Entities;
using Api.Features.Slides.Persistence;
using Common.Enums;
using Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

public partial class SlidesController : ApiControllerBase
{
    [HttpGet("/api/presentations/{presentationId:guid}/slides/{index:int}")]
    public async Task<IActionResult> GetByIndex([FromRoute] Guid presentationId, [FromRoute] int index)
    {
        try
        {
            return Ok(await Mediator.Send(new GetByIndexQuery
            {
                PresentationId = presentationId,
                Index = index
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

public class GetByIndexQuery : IRequest<Slide>
{
    public Guid PresentationId { get; init; }
    public int Index { get; set; }
}

public class GetByIndexQueryHandler(ISlidesRepository repository) : IRequestHandler<GetByIndexQuery, Slide>
{
    public async Task<Slide> Handle(GetByIndexQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetSlideByIndex(request.PresentationId, request.Index, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Slide);

        return result;
    }
}
