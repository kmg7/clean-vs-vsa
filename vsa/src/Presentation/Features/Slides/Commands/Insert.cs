namespace Api.Features.Slides.Commands;

using Api.Common;
using Api.Common.Enums;
using Api.Common.Exceptions;
using Api.Features.Presentations.Persistence;
using Api.Features.Slides.Persistence;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public partial class SlidesController : ApiControllerBase
{
    [HttpPost("/api/presentation/{presentationId:guid}/slides/{index:int}")]
    public async Task<IActionResult> Insert([FromRoute] Guid presentationId, [FromRoute] int index, [FromBody] CreateSlideRequest request)
    {
        try
        {
            var response = await Mediator.Send(new InsertCommand
            {
                PresentationId = presentationId,
                Index = index,
                Content = request.Content,
                Type = request.Type
            });

            return Created($"/api/presentation/{presentationId}/slides/{response.Index}", response);
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

public class InsertCommand : IRequest<Slide>
{
    public Guid PresentationId { get; init; }
    public int Index { get; set; }
    public string Type { get; set; }
    public JsonDocument Content { get; init; }
}

public class InsertCommandHandler(IPresentationsRepository presentationsRepository,
    ISlidesRepository SlidesRepository) : IRequestHandler<InsertCommand, Slide>
{
    public async Task<Slide> Handle(InsertCommand request, CancellationToken cancellationToken)
    {
        if (!await presentationsRepository.PresentationExists(request.PresentationId, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Presentation);
        }

        return await SlidesRepository
            .InsertSlide(request.PresentationId, request.Index, request.Type, request.Content, cancellationToken);
    }
}
