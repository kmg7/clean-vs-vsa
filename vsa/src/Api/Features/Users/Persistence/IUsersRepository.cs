namespace Api.Features.Users.Persistence;

using Api.Features.Users.Entities;
using System.Threading.Tasks;

public interface IUsersRepository
{
    Task<List<User>> GetUsers(CancellationToken cancellationToken);

    Task<User> GetUserById(Guid id, CancellationToken cancellationToken);

    Task<bool> UserExists(Guid id, CancellationToken cancellationToken);
}
