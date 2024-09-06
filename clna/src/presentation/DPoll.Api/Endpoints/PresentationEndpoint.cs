using Dpoll.Api.Filters;
using Dpoll.Api.Requests;
using Dpoll.Domain.Entities;
using DPoll.Api.Extensions;
using DPoll.Application.Features.Presentations.Commands;
using DPoll.Application.Features.Presentations.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dpoll.Api.Endpoints;
public static class PresentationEndpoints
{
    public static WebApplication MapPresentationEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/presentation")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .WithTags("presentation")
            .WithDescription("Lookup, Find and Manipulate Presentations")
            .WithOpenApi();

        _ = root.MapGet("/", GetPresentations)
            .Produces<List<Presentation>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup all Presentations")
            .WithDescription("\n    GET /presentation");

        _ = root.MapGet("/{id}", GetPresentationById)
            .Produces<Presentation>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup a Presentation by its Id")
            .WithDescription("\n    GET /presentation/00000000-0000-0000-0000-000000000000");

        _ = root.MapPost("/", CreatePresentation)
           .Produces<Presentation>(StatusCodes.Status201Created)
           .ProducesProblem(StatusCodes.Status500InternalServerError)
           .ProducesValidationProblem()
           .WithSummary("Create a Presentation")
           .WithDescription("\n    POST /presentation\n     {         \"userId\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\",         \"title\": \"Awesome Movies to Share with my fellow coworkers\"}");

        _ = root.MapPatch("/{id}", UpdatePresentation)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Update a Presentation")
            .WithDescription("\n    PUT /presentation/00000000-0000-0000-0000-000000000000\n     {          \"userId\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\",         \"title\": \"Awesome Movies to Share with my fellow coworkers\"}");

        _ = root.MapDelete("/{id}", DeletePresentation)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a Presentation by its Id")
            .WithDescription("\n    DELETE /presentation/00000000-0000-0000-0000-000000000000");

        return app;
    }

    public static async Task<IResult> GetPresentations([FromServices] IMediator mediator)
    {
        var result = await mediator.Send(new GetPresentationsQuery());
        return result.Ok200Response();
    }

    public static async Task<IResult> GetPresentationById([Validate][FromRoute] Guid id, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(new GetPresentationByIdQuery { Id = id });
        return result.Ok200Response();
    }

    public static async Task<IResult> CreatePresentation([FromBody] CreatePresentationRequest presentation, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(new CreatePresentationCommand
        {
            UserId = presentation.UserId,
            Title = presentation.Title
        });

        return result.Created201Response($"/api/presentation/{result.Value.Id}");
    }

    public static async Task<IResult> UpdatePresentation([Validate][FromRoute] Guid id, [Validate][FromBody] UpdatePresentationRequest request, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(new UpdatePresentationCommand
        {
            Id = id,
            UserId = request.UserId,
            Title = request.Title
        });

        return result.NoContent204Response();
    }

    public static async Task<IResult> DeletePresentation([Validate][FromRoute] Guid id, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(new DeletePresentationCommand
        {
            Id = id,
        });

        return result.NoContent204Response();
    }
}

