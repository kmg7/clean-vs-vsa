using Dpoll.Api.Filters;
using Dpoll.Domain.Common.Exceptions;
using Dpoll.Domain.Entities;
using DPoll.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Queries = DPoll.Application.Features.Users.Queries;


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
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup a User by their Id")
            .WithDescription("\n    GET /User/00000000-0000-0000-0000-000000000000");

        return app;
    }

    public static async Task<IResult> GetUsers([FromServices] IMediator mediator)
    {
        var response = await mediator.Send(new Queries.GetUsers.GetUsersQuery());
        return response.ToHttpResponse();
    }

    public static async Task<IResult> GetUserById([Validate][FromRoute] Guid id, [FromServices] IMediator mediator)
    {
        var response = await mediator.Send(new Queries.GetUserById.GetUserByIdQuery
        {
            Id = id
        });

        return response.ToHttpResponse();
    }
}
