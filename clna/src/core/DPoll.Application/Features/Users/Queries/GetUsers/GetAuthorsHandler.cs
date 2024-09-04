using Dpoll.Domain.Entities;
using DPoll.Application.Features.Users;
using MediatR;

namespace DPoll.Application.Features.Users.Queries.GetUsers;
public class GetUsersHandler(IUsersRepository repository) : IRequestHandler<GetUsersQuery, List<User>>
{
    public async Task<List<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetUsers(cancellationToken);
    }
}
