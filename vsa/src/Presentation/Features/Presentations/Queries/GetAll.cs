namespace Api.Features.Presentations.Queries;

using Api.Common;
using Api.Features.Presentations.Persistence;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

public partial class PresentationsController : ApiControllerBase
{
    [HttpGet("/api/presentations")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            return Ok(await Mediator.Send(new GetAllQuery()));
        }
        catch (Exception ex)
        {
            return Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}

public class GetAllQuery : IRequest<List<Presentation>> { }

public class GetAllQueryHandler(IPresentationsRepository repository) : IRequestHandler<GetAllQuery, List<Presentation>>
{
    public async Task<List<Presentation>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetPresentations(cancellationToken);
    }
}
