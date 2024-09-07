using Dpoll.Domain.Entities;
using DPoll.Application.Features.Users;
using Microsoft.EntityFrameworkCore;

namespace DPoll.Persistence.Databases.UserPresentations;
internal class EFUsersRepository : IUsersRepository
{
    private readonly DPollDbContext context;

    public EFUsersRepository(DPollDbContext context, TimeProvider timeProvider)
    {
        this.context = context;
        ArgumentNullException.ThrowIfNull(context);
    }

    public virtual async Task<List<User>> GetUsers(CancellationToken token)
    {
        var users = await context.Users
            .AsNoTracking()
            .ToListAsync(token);

        return users;
    }

    public virtual async Task<User?> GetUserById(Guid id, CancellationToken token)
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(token);

        return user;
    }

    public virtual async Task<bool> UserExists(Guid id, CancellationToken token)
    {
        var exists = await context.Users.
            AsNoTracking().
            AnyAsync(a => a.Id == id, token);

        return exists;
    }
}
