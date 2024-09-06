namespace Api.Features.Slides.Commands;

using Api.Common;
using Api.Common.Enums;
using Api.Common.Exceptions;
using Api.Features.Presentations.Persistence;
using Api.Features.Slides.Entities;
using Api.Features.Slides.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public partial class SlidesController : ApiControllerBase
{
    [HttpPost("/api/presentation/{presentationId:guid}/slides")]
    public async Task<IActionResult> Create([FromRoute] Guid presentationId, [FromBody] CreateSlideRequest request)
    {
        try
        {
            var response = await Mediator.Send(new CreateCommand
            {
                PresentationId = presentationId,
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

public class CreateCommand : IRequest<Slide>
{
    public Guid PresentationId { get; init; }
    public string Type { get; set; }
    public JsonDocument Content { get; init; }
}

public class CreateSlideRequest
{
    public string Type { get; set; }
    public JsonDocument Content { get; set; }
}

public class CreateSlideValidator : AbstractValidator<CreateSlideRequest>
{
    public CreateSlideValidator()
    {

        _ = RuleFor(r => r.Type)
            .NotEqual(string.Empty)
            .WithMessage("A Slide type was not supplied to create the Slide.");

        // TODO slide content validation here
    }
}

public class CreateCommandHandler(IPresentationsRepository presentationsRepository,
    ISlidesRepository slidesRepository) : IRequestHandler<CreateCommand, Slide>
{
    public async Task<Slide> Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        if (!await presentationsRepository.PresentationExists(request.PresentationId, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Presentation);
        }

        return await slidesRepository
            .CreateSlide(request.PresentationId, request.Type, request.Content, cancellationToken);
    }
}
