namespace Api.Endpoints;

using Api.Requests;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public static class ContentEndpoints
{
    public static WebApplication MapContentEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/presentation/content")
            .WithTags("presentationcontent")
            .WithDescription("Presentation Contents")
            .WithOpenApi();

        _ = root.MapGet("/", GetPresentationContents)
            .Produces<List<PresentationContent>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup all Presentation Contents")
            .WithDescription("\n    GET /presentationcontent");

        _ = root.MapGet("/{id}", GetPresentationContentById)
            .Produces<PresentationContent>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup a Presentation Content by its Id")
            .WithDescription("\n    GET /presentationcontent/00000000-0000-0000-0000-000000000000");

        _ = root.MapPost("/", PostPresentationContent)
            .Produces<PresentationContent>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a new Presentation Content")
            .WithDescription("\n    POST /presentationcontent");

        _ = root.MapDelete("/{id}", DeletePresentationContentById)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a Presentation Content by its Id")
            .WithDescription("\n    DELETE /presentationcontent/00000000-0000-0000-0000-000000000000");

        return app;
    }

    public static async Task<IResult> GetPresentationContents([FromServices] PgDbContext _context)
    {
        try
        {
            var contents = await _context.PresentationContents.ToListAsync();
            return Results.Ok(contents);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> GetPresentationContentById([FromServices] PgDbContext _context, Guid id)
    {
        try
        {
            var content = await _context.PresentationContents.FindAsync(id);
            if (content == null)
                return Results.NotFound();

            return Results.Ok(content);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> PostPresentationContent([FromServices] PgDbContext _context, [FromBody] CreatePresentationContentRequest request)
    {
        try
        {
            var content = await _context.PresentationContents.AddAsync(new PresentationContent
            {
                PresentationId = request.PresentationId,
                Title = request.Title,
                Slides = request.Slides,
            });
            await _context.SaveChangesAsync();
            return Results.Json(content.Entity, statusCode: 201);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> DeletePresentationContentById([FromServices] PgDbContext _context, Guid id)
    {
        try
        {
            var content = await _context.PresentationContents.FindAsync(id);
            if (content == null)
                return Results.NotFound();

            _context.PresentationContents.Remove(content);
            await _context.SaveChangesAsync();
            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}

