namespace Api.Features.Users.Queries;

using Api.Common;
using Api.Features.Users.Entities;
using Api.Features.Users.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

public partial class UsersController : ApiControllerBase
{
    [HttpGet("/api/users")]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            return Ok(await Mediator.Send(new GetAllQuery()));
        }
        catch (Exception ex)
        {
            return Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}

public class GetAllQuery : IRequest<List<User>> { }

public class GetAllQueryHandler(IUsersRepository repository) : IRequestHandler<GetAllQuery, List<User>>
{
    public async Task<List<User>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetUsers(cancellationToken);
    }
}
