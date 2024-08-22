using Application.Slides.Entities;
using System.Text.Json;

namespace Application.Slides;
public interface ISlidesRepository
{
    Task<Slide> CreateSlide(
        Guid presentationId,
        string type,
        JsonDocument slide,
        CancellationToken cancellationToken);

    Task<Slide> InsertSlide(
        Guid presentationId,
        int index,
        string type,
        JsonDocument slide,
        CancellationToken cancellationToken);

    Task<List<Slide>> GetSlides(Guid presentationId, CancellationToken cancellationToken);
    Task<Slide> GetSlideById(Guid id, CancellationToken cancellationToken);
    Task<Slide> GetSlideByIndex(Guid presentationId, int index, CancellationToken cancellationToken);
    Task<bool> SlideExists(Guid id, CancellationToken cancellationToken);

    Task<bool> UpdateSlide(
        Guid id,
        string type,
        bool isVisible,
        JsonDocument content,
        CancellationToken cancellationToken);

    Task<bool> UpdateSlideIndex(
        Guid id,
        int index,
        CancellationToken cancellationToken);
    Task<bool> DeleteSlide(Guid id, CancellationToken cancellationToken);
}
