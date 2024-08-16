namespace Api.Endpoints;

using Api.Requests;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public static class PresentationSessionsEndpoints
{
    public static WebApplication MapPresentationSessionEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/presentationsession")
            .WithTags("presentationsession")
            .WithDescription("Presentation Sessions")
            .WithOpenApi();

        _ = root.MapGet("/", GetPresentationSessions)
            .Produces<List<PresentationSession>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup all Presentation Sessions")
            .WithDescription("\n    GET /presentationsession");

        _ = root.MapGet("/{id}", GetPresentationSessionById)
            .Produces<PresentationSession>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup a Presentation Session by its Id")
            .WithDescription("\n    GET /presentationsession/00000000-0000-0000-0000-000000000000");

        _ = root.MapPost("/", PostPresentationSession)
            .Produces<PresentationSession>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a new Presentation Session")
            .WithDescription("\n    POST /presentationsession");

        _ = root.MapDelete("/{id}", DeletePresentationSessionById)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a Presentation Session by its Id")
            .WithDescription("\n    DELETE /presentationsession/00000000-0000-0000-0000-000000000000");

        return app;
    }

    public static async Task<IResult> GetPresentationSessions([FromServices] PgDbContext _context)
    {
        try
        {
            var sessions = await _context.PresentationSessions.ToListAsync();
            return Results.Ok(sessions);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> GetPresentationSessionById([FromServices] PgDbContext _context, Guid id)
    {
        try
        {
            var session = await _context.PresentationSessions.FindAsync(id);
            if (session == null)
                return Results.NotFound();

            return Results.Ok(session);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> PostPresentationSession([FromServices] PgDbContext _context, [FromBody] CreatePresentationSessionRequest request)
    {
        try
        {
            var session = await _context.PresentationSessions.AddAsync(new PresentationSession
            {
                PresentationId = request.PresentationId,
                Title = request.Title,
                RoomCode = request.RoomCode,
            });
            await _context.SaveChangesAsync();
            return Results.Json(session.Entity, statusCode: 201);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> DeletePresentationSessionById([FromServices] PgDbContext _context, Guid id)
    {
        try
        {
            var session = await _context.PresentationSessions.FindAsync(id);
            if (session == null)
                return Results.NotFound();

            _context.PresentationSessions.Remove(session);
            await _context.SaveChangesAsync();
            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}

