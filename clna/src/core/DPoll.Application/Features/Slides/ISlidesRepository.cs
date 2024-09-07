using Dpoll.Domain.Entities;
using System.Linq.Expressions;

namespace DPoll.Application.Features.Slides;
public interface ISlidesRepository
{
    Task<Slide> CreateSlide(Slide slide, CancellationToken token);
    Task<IQueryable<Slide>> GetSlides(Expression<Func<Slide, bool>> predicate, CancellationToken token);
    Task<Slide?> GetSlideById(Guid id, CancellationToken token);
    Task<Slide?> GetSlideByIndex(Guid presentationId, int index, CancellationToken token);
    Task<int> GetLastSlideIndex(Guid presentationId, CancellationToken token);
    Task<bool> SlideExists(Guid id, CancellationToken token);
    Task<bool> UpdateSlide(Slide slide, CancellationToken token);
    Task<bool> UpdateSlideRange(IQueryable<Slide> slides, CancellationToken token);
    Task<bool> DeleteSlide(Guid id, CancellationToken token);
}
