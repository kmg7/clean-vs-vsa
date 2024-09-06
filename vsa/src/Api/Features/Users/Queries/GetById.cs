namespace Api.Features.Users.Queries;

using Api.Common;
using Api.Common.Enums;
using Api.Common.Exceptions;
using Api.Features.Users.Entities;
using Api.Features.Users.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

public partial class UsersController : ApiControllerBase
{
    [HttpGet("/api/users/{id:guid}")]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id)
    {
        try
        {
            return Ok(await Mediator.Send(new GetByIdQuery
            {
                Id = id
            }));
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}

public class GetByIdQuery : IRequest<User>
{
    public Guid Id { get; init; }
}

public class GetByIdQueryQueryHandler(IUsersRepository repository) : IRequestHandler<GetByIdQuery, User>
{
    public async Task<User> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetUserById(request.Id, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.User);

        return result;
    }
}
