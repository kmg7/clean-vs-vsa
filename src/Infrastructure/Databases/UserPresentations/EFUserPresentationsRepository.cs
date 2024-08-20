namespace Infrastructure.Databases.UserPresentations;

using System;
using Application.Users;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Presentations;
using AutoMapper;
using Extensions;
using Microsoft.EntityFrameworkCore;
using Models;
using ApplicationUser = Application.Users.Entities.User;
using ApplicationPresentation = Application.Presentations.Entities.Presentation;

internal class EFUserPresentationsRepository : IUsersRepository, IPresentationsRepository
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

    public virtual async Task<List<ApplicationUser>> GetUsers(CancellationToken cancellationToken)
    {
        var users = await context.Users
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return mapper.Map<List<ApplicationUser>>(users);
    }

    public virtual async Task<ApplicationUser> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return mapper.Map<ApplicationUser>(user);
        ;
    }

    public virtual async Task<bool> UserExists(Guid id, CancellationToken cancellationToken)
    {
        return await context.Users.AsNoTracking().AnyAsync(a => a.Id == id, cancellationToken);
    }

    #endregion Users

    #region Presentations

    public async Task<ApplicationPresentation> CreatePresentation(
        Guid userId,
        string title,
        CancellationToken cancellationToken)
    {
        var Presentation = new Presentation
        {
            UserId = userId,
            Title = title,
            CreatedAt = timeProvider.GetUtcNow().UtcDateTime,
            UpdatedAt = timeProvider.GetUtcNow().UtcDateTime,
            Slides = []
        };

        var id = context.Add(Presentation).Entity.Id;

        _ = await context.SaveChangesAsync(cancellationToken);

        var result = await context.Presentations
            .Where(r => r.Id == id)
            .AsNoTracking()
            .FirstAsync(cancellationToken);

        return mapper.Map<ApplicationPresentation>(result);
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

    public async Task<List<ApplicationPresentation>> GetPresentations(CancellationToken cancellationToken)
    {
        var result = await context.Presentations
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return mapper.Map<List<ApplicationPresentation>>(result);
    }

    public async Task<ApplicationPresentation> GetPresentationById(Guid id, CancellationToken cancellationToken)
    {
        var result = await context.Presentations
            .Where(r => r.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return mapper.Map<ApplicationPresentation>(result);
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

    //#region Slides

    //public virtual async Task<List<ApplicationSlide>> GetSlides(CancellationToken cancellationToken)
    //{
    //    var result = await context.Slides
    //        .Include(m => m.Presentations)
    //        .ThenInclude(r => r.PresentationUser)
    //        .AsNoTracking()
    //        .ToListAsync(cancellationToken);

    //    return mapper.Map<List<ApplicationSlide>>(result);
    //}

    //public virtual async Task<ApplicationSlide> GetSlideById(Guid id, CancellationToken cancellationToken)
    //{
    //    var result = await context.Slides
    //        .Where(r => r.Id == id)
    //        .Include(m => m.Presentations)
    //        .ThenInclude(r => r.PresentationUser)
    //        .AsNoTracking()
    //        .FirstOrDefaultAsync(cancellationToken);

    //    return mapper.Map<ApplicationSlide>(result);
    //}

    //public virtual async Task<bool> SlideExists(Guid id, CancellationToken cancellationToken)
    //{
    //    return await context.Slides.AsNoTracking().AnyAsync(m => m.Id == id, cancellationToken);
    //}

    //#endregion Slides

}
