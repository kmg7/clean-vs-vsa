using Api.Common.Enums;
using Api.Common.Exceptions;
using Api.Common.Persistence;
using Api.Features.Presentations.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Presentations.Persistence;

public class EfPresentationRepository(BaseDbContext context, IMapper mapper, TimeProvider timeProvider) : IPresentationsRepository
{
    public async Task<Presentation> CreatePresentation(
    Guid userId,
    string title,
    CancellationToken cancellationToken)
    {
        var presentation = new Presentation
        {
            UserId = userId,
            Title = title,
            CreatedAt = timeProvider.GetUtcNow().UtcDateTime,
            UpdatedAt = timeProvider.GetUtcNow().UtcDateTime,
        };

        var id = context.Add(presentation).Entity.Id;

        _ = await context.SaveChangesAsync(cancellationToken);

        var result = await context.Presentations
            .Where(r => r.Id == id)
            .AsNoTracking()
            .FirstAsync(cancellationToken);

        return mapper.Map<Presentation>(result);
    }

    public async Task<bool> DeletePresentation(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            _ = context.Remove(context.Presentations.Single(r => r.Id == id));
            _ = await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public async Task<List<Presentation>> GetPresentations(CancellationToken cancellationToken)
    {
        var result = await context.Presentations
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return mapper.Map<List<Presentation>>(result);
    }

    public async Task<Presentation> GetPresentationById(Guid id, CancellationToken cancellationToken)
    {
        var result = await context.Presentations
            .Where(r => r.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return mapper.Map<Presentation>(result);
    }

    public async Task<bool> PresentationExists(Guid id, CancellationToken cancellationToken)
    {
        return await context.Presentations.AnyAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<bool> UpdatePresentation(
        Guid id,
        Guid userId,
        string title,
        CancellationToken cancellationToken)
    {
        try
        {
            var presentation = context.Presentations.FirstOrDefault(r => r.Id == id);

            NotFoundException.ThrowIfNull(presentation, EntityType.Presentation);

            presentation.Title = title;
            presentation.UserId = userId;
            presentation.UpdatedAt = timeProvider.GetUtcNow().UtcDateTime;

            _ = context.Update(presentation);
            _ = await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
}
