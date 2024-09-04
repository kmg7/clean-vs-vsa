using Dpoll.Api.Filters;
using Dpoll.Api.Requests;
using Dpoll.Domain.Common.Exceptions;
using Dpoll.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Commands = DPoll.Application.Features.Slides.Commands;
using Queries = DPoll.Application.Features.Slides.Queries;

namespace Dpoll.Api.Endpoints;
public static class SlideEndpoints
{
    public static WebApplication MapSlideEndpoints(this WebApplication app)
    {
        var presentationGroup = app.MapGroup("/api/presentation/{presentationId:guid}/slides")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .WithTags("PresentationSlides")
            .WithDescription("Lookup, Find and Manipulate Presentation Slides")
            .WithOpenApi();

        _ = presentationGroup.MapGet("/", GetSlides)
            .Produces<List<Slide>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup all Presentation Slides")
            .WithDescription("\n    GET /presentation/Slide");

        _ = presentationGroup.MapGet("/{index:int}", GetSlideByIndex)
            .Produces<Slide>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup a Presentation Slide by its index")
            .WithDescription("\n    GET /presentation/slide/00000000-0000-0000-0000-000000000000");

        _ = presentationGroup.MapPost("/", CreateSlide)
            .Produces<Slide>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a new Presentation Slide")
            .WithDescription("\n    POST /presentation/slide");

        _ = presentationGroup.MapPost("/{index:int}", InsertSlide)
            .Produces<Slide>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a new Presentation Slide")
            .WithDescription("\n    POST /presentation/slide");

        var slideGroup = app.MapGroup("/api/slide/{id:guid}")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .WithTags("Slides")
            .WithDescription("Create, Update and Delete Presentation Slides")
            .WithOpenApi();

        _ = slideGroup.MapPatch("/", UpdateSlide)
            .Produces<Slide>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Perform JsonPath operations to a Presentation Slide by its index")
            .WithDescription("\n    PATCH /presentation/Slide/00000000-0000-0000-0000-000000000000");

        _ = slideGroup.MapPatch("/index", UpdateSlideIndex)
            .Produces<Slide>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Perform JsonPath operations to a Presentation Slide by its index")
            .WithDescription("\n    PATCH /presentation/Slide/00000000-0000-0000-0000-000000000000");

        _ = slideGroup.MapDelete("/", DeleteSlide)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a Presentation Slide by its Id")
            .WithDescription("\n    DELETE /presentation/slide/00000000-0000-0000-0000-000000000000");

        return app;
    }

    public static async Task<IResult> GetSlides([Validate][FromRoute] Guid presentationId, [FromServices] IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(new Queries.GetSlides.GetSlidesQuery
            {
                PresentationId = presentationId
            }));
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> GetSlideByIndex([Validate][FromRoute] Guid presentationId, int index, [FromServices] IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(new Queries.GetSlideByIndex.GetSlideByIndexQuery
            {
                PresentationId = presentationId,
                Index = index
            }));
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> CreateSlide([Validate][FromRoute] Guid presentationId, [FromBody] CreateSlideRequest request, [FromServices] IMediator mediator)
    {
        try
        {
            var response = await mediator.Send(new Commands.CreateSlide.CreateSlideCommand
            {
                PresentationId = presentationId,
                Content = request.Content,
                Type = request.Type
            });

            return Results.Created($"/api/presentation/{presentationId}/slides/{response.Index}", response);
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> InsertSlide([Validate][FromRoute] Guid presentationId, [FromRoute] int index, [FromBody] CreateSlideRequest request, [FromServices] IMediator mediator)
    {
        try
        {
            var response = await mediator.Send(new Commands.InsertSlide.InsertSlideCommand
            {
                PresentationId = presentationId,
                Index = index,
                Content = request.Content,
                Type = request.Type
            });

            return Results.Created($"/api/presentation/{presentationId}/slides/{response.Index}", response);
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> UpdateSlide([Validate][FromRoute] Guid id, [Validate][FromBody] UpdateSlideRequest request, [FromServices] IMediator mediator)
    {
        try
        {
            _ = await mediator.Send(new Commands.UpdateSlide.UpdateSlideCommand
            {
                Id = id,
                Type = request.Type,
                Content = request.Content,
                IsVisible = request.IsVisible
            });

            return Results.NoContent();
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
    public static async Task<IResult> UpdateSlideIndex([Validate][FromRoute] Guid id, [Validate][FromBody] UpdateSlideIndexRequest request, [FromServices] IMediator mediator)
    {
        try
        {
            _ = await mediator.Send(new Commands.UpdateSlideIndex.UpdateSlideIndexCommand
            {
                Id = id,
                Index = request.Index
            });

            return Results.NoContent();
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> DeleteSlide([Validate][FromRoute] Guid id, [FromServices] IMediator mediator)
    {
        try
        {
            _ = await mediator.Send(new Commands.DeleteSlide.DeleteSlideCommand
            {
                Id = id,
            });

            return Results.NoContent();
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}
