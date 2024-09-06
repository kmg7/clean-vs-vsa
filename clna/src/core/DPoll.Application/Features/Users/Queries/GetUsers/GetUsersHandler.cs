using Dpoll.Domain.Entities;
using DPoll.Application.Shared;
using MediatR;

namespace DPoll.Application.Features.Users.Queries.GetUsers;
public class GetUsersHandler(IUsersRepository repository) : IRequestHandler<GetUsersQuery, Result<List<User>>>
{
    public async Task<Result<List<User>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await repository.GetUsers(cancellationToken);
        var result = Result<List<User>>.Success(users);
        return result;
    }
}
