using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Entities;
using DPoll.Application.Shared;
using DPoll.Domain.Common.Errors;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace DPoll.Application.Features.Users.Queries;
public class GetUserByIdQuery : IRequest<Result<User>>
{
    [Required]
    public Guid Id { get; init; }
}

public class GetUserByIdHandler(IUsersRepository repository) : IRequestHandler<GetUserByIdQuery, Result<User>>
{
    public async Task<Result<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await repository.GetUserById(request.Id, cancellationToken);
        if (user is null)
            return Result<User>.Failure(Error.NotFound(EntityType.User));

        var result = Result<User>.Success(user);

        return result;
    }
}
