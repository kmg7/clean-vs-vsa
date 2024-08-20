namespace Application.Users.Queries.GetUsers;

using System.Threading;
using System.Threading.Tasks;
using Entities;
using MediatR;

public class GetUsersHandler(IUsersRepository repository) : IRequestHandler<GetUsersQuery, List<User>>
{
    public async Task<List<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetUsers(cancellationToken);
    }
}
