using Dpoll.Api.Filters;
using Dpoll.Domain.Entities;
using DPoll.Api.Extensions;
using DPoll.Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Dpoll.Api.Endpoints;
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
            .Produces<List<User>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup all Users")
            .WithDescription("\n    GET /user");

        _ = root.MapGet("/{id}", GetUserById)
            .Produces<User>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup a User by their Id")
            .WithDescription("\n    GET /User/00000000-0000-0000-0000-000000000000");

        return app;
    }

    public static async Task<IResult> GetUsers([FromServices] IMediator mediator)
    {
        var response = await mediator.Send(new GetUsersQuery());
        return response.Ok200Response();
    }

    public static async Task<IResult> GetUserById([Validate][FromRoute] Guid id, [FromServices] IMediator mediator)
    {
        var response = await mediator.Send(new GetUserByIdQuery
        {
            Id = id
        });

        return response.Ok200Response();
    }
}
