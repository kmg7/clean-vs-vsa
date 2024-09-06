using Api.Common.Persistence;
using Api.Features.Users.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Users.Persistence;

public class EfUserRepository(BaseDbContext context, IMapper mapper) : IUsersRepository
{
    public virtual async Task<List<User>> GetUsers(CancellationToken cancellationToken)
    {
        var users = await context.Users
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return mapper.Map<List<User>>(users);
    }

    public virtual async Task<User> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return mapper.Map<User>(user);
    }

    public virtual async Task<bool> UserExists(Guid id, CancellationToken cancellationToken)
    {
        return await context.Users.AsNoTracking().AnyAsync(a => a.Id == id, cancellationToken);
    }
}
