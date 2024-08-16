namespace Api.Endpoints;

using Api.Requests;
using API.Models;
using API.Services;
using Json.More;
using Json.Patch;
using Json.Pointer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public static class SlideEndpoints
{
    public static WebApplication MapSlideEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/presentation/content/{id:guid}/slides")
            .WithTags("presentationcontentlides")
            .WithDescription("A Presentation Contents Slides")
            .WithOpenApi();

        _ = root.MapGet("/", GetSlides)
            .Produces<List<PresentationContent>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup all Presentation Contents")
            .WithDescription("\n    GET /presentation/content");

        _ = root.MapGet("/{index}", GetSlideByIndex)
            .Produces<PresentationContent>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup a Presentation Content by its index")
            .WithDescription("\n    GET /presentation/content/00000000-0000-0000-0000-000000000000");

        _ = root.MapPost("/", PostSlide)
            .Produces<PresentationContent>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a new Presentation Content")
            .WithDescription("\n    POST /presentation/content");

        _ = root.MapPatch("/", PatchSlide)
            .Produces<PresentationContent>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Perform JsonPath operations to a Presentation Content by its index")
            .WithDescription("\n    PATCH /presentation/content/00000000-0000-0000-0000-000000000000");

        _ = root.MapDelete("/", DeleteSlideByIndex)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a Presentation Content by its Id")
            .WithDescription("\n    DELETE /presentation/content/00000000-0000-0000-0000-000000000000");

        return app;
    }
    public static async Task<IResult> PatchSlide([FromServices] PgDbContext _context, Guid id, [FromBody] JsonPatch patch)
    {
        try
        {
            var content = await _context.PresentationContents.FindAsync(id);
            if (content == null)
                return Results.NotFound();

            var patched = patch.Apply(content.Slides);
            content.Slides = patched.ToJsonDocument();

            _context.PresentationContents.Update(content);
            await _context.SaveChangesAsync();
            return Results.Json(content.Slides, statusCode: 201);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> GetSlides([FromServices] PgDbContext _context, Guid id)
    {
        try
        {
            var content = await _context.PresentationContents.FindAsync(id);
            if (content == null)
                return Results.NotFound();

            return Results.Ok(content.Slides);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> GetSlideByIndex([FromServices] PgDbContext _context, Guid id, string index)
    {
        try
        {
            var content = await _context.PresentationContents.FindAsync(id);
            if (content == null)
                return Results.NotFound();

            var x = JsonPointer.Parse($"/{index}");

            var slide = x.Evaluate(content.Slides.RootElement);
            if (slide == null)
                return Results.NotFound();

            return Results.Ok(content);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> PostSlide([FromServices] PgDbContext _context, Guid id, [FromBody] CreateSlideRequest request)
    {
        try
        {
            var content = await _context.PresentationContents.FindAsync(id);
            if (content == null)
                return Results.NotFound();

            var p = JsonPointer.Parse($"/{request.Index}");
            var patch = new JsonPatch(PatchOperation.Add(p, request.Slide));
            var patched = patch.Apply(content.Slides);
            content.Slides = patched.ToJsonDocument();

            _context.PresentationContents.Update(content);
            await _context.SaveChangesAsync();
            return Results.Json(content.Slides, statusCode: 201);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }



    public static async Task<IResult> DeleteSlideByIndex([FromServices] PgDbContext _context, Guid id, [FromBody] CreateSlideRequest request)
    {
        try
        {
            var content = await _context.PresentationContents.FindAsync(id);
            if (content == null)
                return Results.NotFound();

            var p = JsonPointer.Parse($"/{request.Index}");
            var patch = new JsonPatch(PatchOperation.Remove(p));
            var patched = patch.Apply(content.Slides);
            content.Slides = patched.ToJsonDocument();

            _context.PresentationContents.Update(content);
            await _context.SaveChangesAsync();
            return Results.Json(content.Slides, statusCode: 201);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}

