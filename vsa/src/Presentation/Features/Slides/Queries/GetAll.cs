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
    [HttpGet("/api/presentations/{presentationId:guid}/slides")]
    public async Task<IActionResult> GetAll([FromRoute] Guid presentationId)
    {
        try
        {
            return Ok(await Mediator.Send(new GetAllQuery
            {
                PresentationId = presentationId
            }));
        }
        catch (Exception ex)
        {
            return Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}

public class GetAllQuery : IRequest<List<Slide>>
{
    public Guid PresentationId { get; init; }
}

public class GetAllQueryHandler(ISlidesRepository repository) : IRequestHandler<GetAllQuery, List<Slide>>
{
    public async Task<List<Slide>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetSlides(request.PresentationId, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Slide);

        return result;
    }
}
