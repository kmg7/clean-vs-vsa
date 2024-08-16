namespace Api.Endpoints;

using Api.Requests;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public static class UserEndpoints
{
    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/user")
            .WithTags("user")
            .WithDescription("Users")
            .WithOpenApi();

        _ = root.MapGet("/", GetUsers)
            .Produces<List<User>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup all Users")
            .WithDescription("\n    GET /user");

        _ = root.MapGet("/{id}", GetUserById)
            .Produces<User>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup a User by their Id")
            .WithDescription("\n    GET /User/00000000-0000-0000-0000-000000000000");

        _ = root.MapPost("/", PostUser)
            .Produces<User>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a new User")
            .WithDescription("\n    POST /User");

        _ = root.MapDelete("/{id}", DeleteUserById)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a User by their Id")
            .WithDescription("\n    DELETE /User/00000000-0000-0000-0000-000000000000");

        return app;
    }

    public static async Task<IResult> GetUsers([FromServices] PgDbContext _context)
    {
        try
        {
            var users = await _context.Users.ToListAsync();
            return Results.Ok(users);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> GetUserById([FromServices] PgDbContext _context, Guid id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return Results.NotFound();

            return Results.Ok(user);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> PostUser([FromServices] PgDbContext _context, [FromBody] CreateUserRequest request)
    {
        try
        {
            var user = await _context.Users.AddAsync(new User
            {
                Username = request.Username,
                EmailAddress = request.EmailAddress,
                ClerkId = request.ClerkId,
                IsActive = true
            });
            await _context.SaveChangesAsync();
            return Results.Json(user.Entity, statusCode: 201);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> DeleteUserById([FromServices] PgDbContext _context, Guid id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return Results.NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}
