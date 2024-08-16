namespace Api.Endpoints;

using Api.Requests;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public static class PresentationsEndpoints
{
    public static WebApplication MapPresentationEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/presentation")
            .WithTags("presentation")
            .WithDescription("Presentations")
            .WithOpenApi();

        _ = root.MapGet("/", GetPresentations)
            .Produces<List<Presentation>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup all Presentations")
            .WithDescription("\n    GET /presentation");

        _ = root.MapGet("/{id}", GetPresentationById)
            .Produces<Presentation>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup a Presentation by its Id")
            .WithDescription("\n    GET /presentation/00000000-0000-0000-0000-000000000000");

        _ = root.MapPost("/", PostPresentation)
            .Produces<Presentation>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a new Presentation")
            .WithDescription("\n    POST /presentation");

        _ = root.MapDelete("/{id}", DeletePresentationById)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a Presentation by its Id")
            .WithDescription("\n    DELETE /presentation/00000000-0000-0000-0000-000000000000");

        return app;
    }

    public static async Task<IResult> GetPresentations([FromServices] PgDbContext _context)
    {
        try
        {
            var presentations = await _context.Presentations.ToListAsync();
            return Results.Ok(presentations);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> GetPresentationById([FromServices] PgDbContext _context, Guid id)
    {
        try
        {
            var presentation = await _context.Presentations.FindAsync(id);
            if (presentation == null)
                return Results.NotFound();

            return Results.Ok(presentation);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> PostPresentation([FromServices] PgDbContext _context, [FromBody] CreatePresentationRequest request)
    {
        try
        {
            var presentation = await _context.Presentations.AddAsync(new Presentation
            {
                UserId = request.UserId,
                Title = request.Title,
            });
            await _context.SaveChangesAsync();
            return Results.Json(presentation.Entity, statusCode: 201);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> DeletePresentationById([FromServices] PgDbContext _context, Guid id)
    {
        try
        {
            var presentation = await _context.Presentations.FindAsync(id);
            if (presentation == null)
                return Results.NotFound();

            _context.Presentations.Remove(presentation);
            await _context.SaveChangesAsync();
            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}

