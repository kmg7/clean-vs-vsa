using Dpoll.Api.Filters;
using Dpoll.Api.Requests;
using Dpoll.Domain.Entities;
using DPoll.Api.Extensions;
using DPoll.Application.Features.Slides.Commands;
using DPoll.Application.Features.Slides.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dpoll.Api.Endpoints;
public static class SlideEndpoints
{
    public static WebApplication MapSlideEndpoints(this WebApplication app)
    {
        var presentationGroup = app.MapGroup("/api/presentation/{presentationId:guid}/slides")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .WithTags("presentation")
            .WithDescription("Lookup, Find and Manipulate Presentation Slides")
            .WithOpenApi();

        _ = presentationGroup.MapGet("/", GetSlides)
            .Produces<List<Slide>>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup all Presentation Slides")
            .WithDescription("\n    GET /presentation/Slide");

        _ = presentationGroup.MapGet("/{index:int}", GetSlideByIndex)
            .Produces<Slide>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup a Presentation Slide by its index")
            .WithDescription("\n    GET /presentation/slide/00000000-0000-0000-0000-000000000000");

        _ = presentationGroup.MapPost("/", CreateSlide)
            .Produces<Slide>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a new Presentation Slide")
            .WithDescription("\n    POST /presentation/slide");

        _ = presentationGroup.MapPost("/{index:int}", InsertSlide)
            .Produces<Slide>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a new Presentation Slide")
            .WithDescription("\n    POST /presentation/slide");

        var slideGroup = app.MapGroup("/api/slide/{id:guid}")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .WithTags("slide")
            .WithDescription("Create, Update and Delete Presentation Slides")
            .WithOpenApi();
        
        _ = slideGroup.MapGet("/", GetSlideById) 
            .Produces<Slide>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup a Presentation Slide by its index")
            .WithDescription("\n    GET /presentation/slide/00000000-0000-0000-0000-000000000000");

        _ = slideGroup.MapPatch("/", UpdateSlide)
            .Produces<Slide>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Perform JsonPath operations to a Presentation Slide by its index")
            .WithDescription("\n    PATCH /presentation/Slide/00000000-0000-0000-0000-000000000000");

        _ = slideGroup.MapPatch("/index", UpdateSlideIndex)
            .Produces<Slide>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Perform JsonPath operations to a Presentation Slide by its index")
            .WithDescription("\n    PATCH /presentation/Slide/00000000-0000-0000-0000-000000000000");

        _ = slideGroup.MapDelete("/", DeleteSlide)
            .Produces(StatusCodes.Status204NoContent) 
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a Presentation Slide by its Id")
            .WithDescription("\n    DELETE /presentation/slide/00000000-0000-0000-0000-000000000000");

        return app;
    }

    public static async Task<IResult> GetSlides([Validate][FromRoute] Guid presentationId, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(new GetSlidesQuery { PresentationId = presentationId });
        return result.Ok200Response();
    }

    public static async Task<IResult> GetSlideByIndex([Validate][FromRoute] Guid presentationId, int index, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(new GetSlideByIndexQuery
        {
            PresentationId = presentationId,
            Index = index
        });
        return result.Ok200Response();
    }

    public static async Task<IResult> CreateSlide([Validate][FromRoute] Guid presentationId, [FromBody] CreateSlideRequest request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(new CreateSlideCommand
        {
            PresentationId = presentationId,
            Content = request.Content,
            Type = request.Type
        });
        return result.Created201Response($"/api/presentation/{presentationId}/slides/{result.Value.Index}");
    }
    public static async Task<IResult> GetSlideById([Validate][FromRoute] Guid id, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(new GetSlideByIdQuery{ Id = id, });
        return result.Ok200Response();
    }
    public static async Task<IResult> InsertSlide([Validate][FromRoute] Guid presentationId, [FromRoute] int index, [FromBody] CreateSlideRequest request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(new InsertSlideCommand
        {
            PresentationId = presentationId,
            IndexOfSlideBefore = index,
            Content = request.Content,
            Type = request.Type
        });
        return result.Created201Response($"/api/presentation/{presentationId}/slides/{result.Value.Index}");
    }

    public static async Task<IResult> UpdateSlide([Validate][FromRoute] Guid id, [Validate][FromBody] UpdateSlideRequest request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(new UpdateSlideCommand
        {
            Id = id,
            Type = request.Type,
            Content = request.Content,
            IsVisible = request.IsVisible
        });
        return result.NoContent204Response();
    }

    public static async Task<IResult> UpdateSlideIndex([Validate][FromRoute] Guid id, [Validate][FromBody] UpdateSlideIndexRequest request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(new UpdateSlideIndexCommand { Id = id, Index = request.Index });
        return result.NoContent204Response();
    }

    public static async Task<IResult> DeleteSlide([Validate][FromRoute] Guid id, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(new DeleteSlideCommand { Id = id, });
        return result.NoContent204Response();
    }
}
