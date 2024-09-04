using Dpoll.Domain.Entities;

namespace DPoll.Application.Features.Users;
public interface IUsersRepository
{
    Task<List<User>> GetUsers(CancellationToken cancellationToken);

    Task<User> GetUserById(Guid id, CancellationToken cancellationToken);

    Task<bool> UserExists(Guid id, CancellationToken cancellationToken);
}
