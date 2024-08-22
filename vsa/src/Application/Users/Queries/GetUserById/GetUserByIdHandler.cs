namespace Application.Users.Queries.GetUserById;

using System.Threading;
using System.Threading.Tasks;
using Common.Enums;
using Common.Exceptions;
using Entities;
using MediatR;

public class GetUserByIdHandler(IUsersRepository repository) : IRequestHandler<GetUserByIdQuery, User>
{
    public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetUserById(request.Id, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.User);

        return result;
    }
}
