using Dpoll.Domain.Entities;
using DPoll.Application.Features.Slides;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DPoll.Persistence.Databases.UserPresentations;
internal class EFSlidesRepository : ISlidesRepository
{
    private readonly DPollDbContext context;

    public EFSlidesRepository(DPollDbContext context)
    {
        this.context = context;

        ArgumentNullException.ThrowIfNull(context);
    }

    public async Task<Slide?> GetSlideById(Guid id, CancellationToken token)
    {
        var slide = await context.Slides
            .Where(r => r.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync(token);

        return slide;
    }

    public async Task<Slide?> GetSlideByIndex(Guid presentationId, int index, CancellationToken token)
    {
        var slide = await context.Slides
            .Where(r => r.PresentationId == presentationId && r.Index == index)
            .AsNoTracking()
            .FirstOrDefaultAsync(token);

        return slide;
    }

    public async Task<bool> SlideExists(Guid id, CancellationToken token)
    {
        var slide = await context.Slides
            .Where(r => r.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync(token);

        return slide != null;
    }

    public async Task<int> GetLastSlideIndex(Guid presentationId, CancellationToken token)
    {
        var lastIndex = 0;
        var slideExists = context.Slides.Any(r => r.PresentationId == presentationId);
        if (slideExists)
            lastIndex = await context.Slides
                .MaxAsync(r => r.Index, token);

        return lastIndex;
    }

    public async Task<Slide> CreateSlide(Slide slide, CancellationToken token)
    {
        var id = context.Slides.Add(slide).Entity.Id;
        _ = await context.SaveChangesAsync(token);

        slide.Id = id;
        return slide;
    }

    public async Task<IQueryable<Slide>> GetSlides(Expression<Func<Slide, bool>> predicate, CancellationToken token)
    {
        var slides = context.Slides
            .AsNoTracking()
            .Where(predicate);

        await Task.Run(() => slides.LoadAsync(token), token);

        return slides;
    }

    public async Task<bool> UpdateSlide(Slide slide, CancellationToken token)
    {
        try
        {
            _ = context.Update(slide);
            _ = await context.SaveChangesAsync(token);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> UpdateSlideRange(IQueryable<Slide> slides, CancellationToken token)
    {
        try
        {
            context.UpdateRange(slides);
            _ = await context.SaveChangesAsync(token);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> DeleteSlide(Guid id, CancellationToken token)
    {
        try
        {
            var slide = context.Slides.Single(r => r.Id == id);
            _ = context.Remove(slide);
            _ = await context.SaveChangesAsync(token);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }
}
