namespace Presentation.Endpoints;

using Application.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;
using Entities = Application.Users.Entities;
using Queries = Application.Users.Queries;

public static class UserEndpoints
{
    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/user")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .WithTags("user")
            .WithDescription("Lookup, Find and Manipulate Users")
            .WithOpenApi();

        _ = root.MapGet("/", GetUsers)
            .Produces<List<Entities.User>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup all Users")
            .WithDescription("\n    GET /user");

        _ = root.MapGet("/{id}", GetUserById)
            .Produces<Entities.User>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup a User by their Id")
            .WithDescription("\n    GET /User/00000000-0000-0000-0000-000000000000");

        return app;
    }

    public static async Task<IResult> GetUsers([FromServices] IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(new Queries.GetUsers.GetUsersQuery()));
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> GetUserById([Validate][FromRoute] Guid id, [FromServices] IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(new Queries.GetUserById.GetUserByIdQuery
            {
                Id = id
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
}
