using AutoMapper;
using Dpoll.Domain.Common.Enums;
using Dpoll.Domain.Common.Exceptions;
using Dpoll.Domain.Entities;
using DPoll.Application.Features.Presentations;
using DPoll.Application.Features.Slides;
using DPoll.Application.Features.Users;
using DPoll.Infrastructure.Databases.UserPresentations.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DPoll.Infrastructure.Databases.UserPresentations;
internal class EFUserPresentationsRepository : IUsersRepository, IPresentationsRepository, ISlidesRepository
{
    private readonly UserPresentationsDbContext context;
    private readonly TimeProvider timeProvider;
    private readonly IMapper mapper;

    public EFUserPresentationsRepository(
        UserPresentationsDbContext context,
        TimeProvider timeProvider,
        IMapper mapper)
    {
        this.context = context;
        this.timeProvider = timeProvider;
        this.mapper = mapper;

        ArgumentNullException.ThrowIfNull(context);

        _ = this.context.Database.EnsureDeleted();
        _ = this.context.Database.EnsureCreated();
        _ = this.context.AddData();
    }

    #region Users

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
        ;
    }

    public virtual async Task<bool> UserExists(Guid id, CancellationToken cancellationToken)
    {
        return await context.Users.AsNoTracking().AnyAsync(a => a.Id == id, cancellationToken);
    }

    #endregion Users

    #region Presentations

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
    #endregion Presentations

    #region Slides
    public async Task<Slide> CreateSlide(Guid presentationId, string type, JsonDocument content, CancellationToken cancellationToken)
    {
        try
        {
            var presentation = await context.Presentations
                        .Where(r => r.Id == presentationId)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(cancellationToken);

            NotFoundException.ThrowIfNull(presentation, EntityType.Presentation);
            var lastIndex = 0;
            var slideExists = context.Slides.Any(r => r.PresentationId == presentationId);
            if (slideExists)
                lastIndex = await context.Slides
                    .MaxAsync(r => r.Index, cancellationToken);

            var slide = new Slide
            {
                PresentationId = presentationId,
                Index = lastIndex + 1,
                Type = type,
                Content = content
            };

            _ = context.Add(slide);
            _ = await context.SaveChangesAsync(cancellationToken);
            return mapper.Map<Slide>(slide);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<Slide> InsertSlide(Guid presentationId, int index, string type, JsonDocument content, CancellationToken cancellationToken)
    {
        var presentation = await context.Presentations
            .Where(r => r.Id == presentationId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        NotFoundException.ThrowIfNull(presentation, EntityType.Presentation);
        var slideBefore = await context.Slides
            .Where(r => r.Index == index)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        NotFoundException.ThrowIfNull(slideBefore, EntityType.Slide);
        var slide = new Slide
        {
            PresentationId = presentationId,
            Index = slideBefore.Index + 1,
            Type = type,
            Content = content
        };

        var slidesAfter = await context.Slides
            .Where(r => r.Index > index)
            .ToListAsync(cancellationToken);

        slidesAfter.ForEach(slide => slide.Index++);
        _ = context.Add(slide);
        _ = await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<Slide>(slide);
    }

    public async Task<List<Slide>> GetSlides(Guid presentationId, CancellationToken cancellationToken)
    {
        var presentation = await context.Presentations
            .Where(r => r.Id == presentationId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        ArgumentNullException.ThrowIfNull(presentation);

        var slides = await context.Slides
            .Where(r => r.PresentationId == presentationId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return mapper.Map<List<Slide>>(slides);
    }

    public async Task<Slide> GetSlideById(Guid id, CancellationToken cancellationToken)
    {
        var slide = await context.Slides
            .Where(r => r.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
        NotFoundException.ThrowIfNull(slide, EntityType.Slide);

        return mapper.Map<Slide>(slide);
    }

    public async Task<Slide> GetSlideByIndex(Guid presentationId, int index, CancellationToken cancellationToken)
    {
        var slide = await context.Slides
            .Where(r => r.PresentationId == presentationId && r.Index == index)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
        NotFoundException.ThrowIfNull(slide, EntityType.Slide);

        return mapper.Map<Slide>(slide);
    }

    public async Task<bool> SlideExists(Guid id, CancellationToken cancellationToken)
    {
        var slide = await context.Slides
            .Where(r => r.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return slide != null;
    }

    public async Task<bool> UpdateSlide(Guid id, string type, bool isVisible, JsonDocument content, CancellationToken cancellationToken)
    {
        try
        {
            var slide = await context.Slides
                       .Where(r => r.Id == id)
                       .FirstOrDefaultAsync(cancellationToken);
            NotFoundException.ThrowIfNull(slide, EntityType.Slide);

            slide.Type = type;
            slide.Content = content;
            slide.IsVisible = isVisible;

            _ = await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> UpdateSlideIndex(Guid id, int index, CancellationToken cancellationToken)
    {
        try
        {
            var slide = await context.Slides
                       .Where(r => r.Id == id)
                       .FirstOrDefaultAsync(cancellationToken);

            NotFoundException.ThrowIfNull(slide, EntityType.Slide);
            IQueryable<Slide> affectedSlides;
            int currentIndex = slide.Index;
            bool isDirectionDownwards = currentIndex < index;
            int indexModify = isDirectionDownwards ? -1 : 1;

            if (isDirectionDownwards)
                affectedSlides = context.Slides.Where(s => s.Index > currentIndex && s.Index <= index);
            else
                affectedSlides = context.Slides.Where(s => s.Index < currentIndex && s.Index >= index);

            await affectedSlides.ForEachAsync(s => s.Index += indexModify, cancellationToken);
            slide.Index = index;

            _ = await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> DeleteSlide(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var slide = await context.Slides
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            ArgumentNullException.ThrowIfNull(slide);

            var slidesAfter = await context.Slides
                .Where(r => r.Index > slide.Index)
                .ToListAsync(cancellationToken);

            slidesAfter.ForEach(s => s.Index--);
            _ = context.Remove(slide);
            _ = await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    #endregion Slides
}
