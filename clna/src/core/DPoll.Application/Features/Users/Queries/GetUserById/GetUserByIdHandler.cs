using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Common.Exceptions;
using Dpoll.Domain.Entities;
using DPoll.Application.Features.Users;
using MediatR;

namespace DPoll.Application.Features.Users.Queries.GetUserById;
public class GetUserByIdHandler(IUsersRepository repository) : IRequestHandler<GetUserByIdQuery, User>
{
    public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetUserById(request.Id, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.User);

        return result;
    }
}
