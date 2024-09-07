using Dpoll.Domain.Entities;
using DPoll.Application.Features.Presentations;
using Microsoft.EntityFrameworkCore;

namespace DPoll.Persistence.Databases.UserPresentations;
internal class EFPresentationsRepository : IPresentationsRepository
{
    private readonly DPollDbContext context;
    private readonly TimeProvider timeProvider;

    public EFPresentationsRepository(DPollDbContext context, TimeProvider timeProvider)
    {
        this.context = context;
        this.timeProvider = timeProvider;

        ArgumentNullException.ThrowIfNull(context);
    }

    public async Task<bool> PresentationExists(Guid id, CancellationToken token)
    {
        return await context.Presentations.AnyAsync(r => r.Id == id, token);
    }

    public async Task<List<Presentation>> GetPresentations(CancellationToken token)
    {
        var presentations = await context.Presentations
            .AsNoTracking()
            .ToListAsync(token);

        return presentations;
    }

    public async Task<Presentation?> GetPresentationById(Guid id, CancellationToken token)
    {
        var presentation = await context.Presentations
            .Where(r => r.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync(token);

        return presentation;
    }

    public async Task<Presentation> CreatePresentation(Presentation presentation, CancellationToken token)
    {
        var id = context.Add(presentation).Entity.Id;
        _ = await context.SaveChangesAsync(token);

        presentation.Id = id;
        return presentation;
    }

    public async Task<bool> DeletePresentation(Guid id, CancellationToken token)
    {
        try
        {
            _ = context.Remove(context.Presentations.Single(r => r.Id == id));
            _ = await context.SaveChangesAsync(token);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> UpdatePresentation(Presentation presentation, CancellationToken token)
    {
        try
        {
            presentation.UpdatedAt = timeProvider.GetUtcNow().UtcDateTime;

            _ = context.Update(presentation);
            _ = await context.SaveChangesAsync(token);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

}
